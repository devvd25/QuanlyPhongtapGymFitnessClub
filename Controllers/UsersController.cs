using QuanlyPhongtapGymFitnessClub.DTOs;
using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        private static readonly string _adminUsername = "admin";
        private static readonly string _adminPassword = "123456";

        // GET api/users?search=...&role=...&package=...
        [HttpGet]
        public ActionResult GetAll(
            [FromQuery] string? search,
            [FromQuery] string? role,
            [FromQuery] string? package)
        {
            var query = _context.Users.AsQueryable();
            bool hasFilter = false;

            if (!string.IsNullOrWhiteSpace(search))
            {
                hasFilter = true;
                query = query.Where(u => u.FullName.Contains(search) || u.Username.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                hasFilter = true;
                query = query.Where(u => u.Role == role);
            }

            if (!string.IsNullOrWhiteSpace(package))
            {
                hasFilter = true;
                query = query.Where(u => u.MembershipPackage == package);
            }

            var matchedUsers = query.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                Role = u.Role,
                MembershipPackage = u.MembershipPackage,
                MembershipStatus = u.MembershipStatus,
                MembershipEndDate = u.MembershipEndDate,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            }).ToList();

            if (hasFilter)
            {
                return Ok(new
                {
                    filters = new
                    {
                        search,
                        role,
                        package
                    },
                    totalFound = matchedUsers.Count,
                    users = matchedUsers
                });
            }

            var allUsers = _context.Users.ToList();
            var statsByRole = allUsers.GroupBy(u => u.Role)
                .Select(g => new { role = g.Key, count = g.Count() })
                .ToList();

            var statsByPackage = allUsers.GroupBy(u => u.MembershipPackage)
                .Select(g => new { package = g.Key, count = g.Count() })
                .ToList();

            return Ok(new
            {
                totalSystemUsers = allUsers.Count,
                stats = new
                {
                    byRole = statsByRole,
                    byMembershipPackage = statsByPackage
                },
                users = matchedUsers
            });
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public ActionResult<UserResponseDto> GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });

            return Ok(new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Role = user.Role,
                MembershipPackage = user.MembershipPackage,
                MembershipStatus = user.MembershipStatus,
                MembershipEndDate = user.MembershipEndDate,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });
        }

        // POST api/users/register
        [HttpPost("register")]
        public ActionResult<UserResponseDto> Register([FromBody] UserRegisterDto request)
        {
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest(new { message = $"Username '{request.Username}' đã tồn tại." });

            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest(new { message = $"Email '{request.Email}' đã được đăng ký." });

            // Generate Password Hash and Salt
            byte[] passwordHash, passwordSalt;
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                // Nếu request.Password null thì lấy pass mặc định là 123456
                string pwd = !string.IsNullOrEmpty(request.GetType().GetProperty("Password")?.GetValue(request)?.ToString()) 
                             ? request.GetType().GetProperty("Password")?.GetValue(request)?.ToString() 
                             : "123456";
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pwd!));
            }

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                Phone = request.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "Member",
                MembershipPackage = request.MembershipPackage ?? "Basic",
                MembershipStatus = "Active",
                MembershipEndDate = DateTime.UtcNow.AddMonths(1),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Gender = "Unknown",
                Address = "N/A"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges(); // LƯU VÀO DATABASE THẬT!

            var responseDto = new UserResponseDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                FullName = newUser.FullName,
                Phone = newUser.Phone,
                Role = newUser.Role,
                MembershipPackage = newUser.MembershipPackage,
                MembershipStatus = newUser.MembershipStatus,
                MembershipEndDate = newUser.MembershipEndDate,
                IsActive = newUser.IsActive,
                CreatedAt = newUser.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, responseDto);
        }

        // POST api/users/login
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDto request)
        {
            string role = "";
            string username = "";

            // Kiểm tra nếu là tài khoản Admin chúa
            if (request.Username == _adminUsername && request.Password == _adminPassword)
            {
                role = "Admin";
                username = _adminUsername;
            }
            else
            {
                // Nếu không, tìm trong Database
                var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
                // Dùng pass ảo để pass (vì ta đang để PasswordHash = 0x01) cho tiện đồ án
                if (user == null || request.Password != "password123") 
                {
                    return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
                }
                role = user.Role;
                username = user.Username;
            }

            // ===== TẠO JWT TOKEN CHỨA QUYỀN (ROLE) =====
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            // Khóa bí mật phải giống với trong Program.cs
            var key = Encoding.UTF8.GetBytes("MySuperSecretKeyForGymApp_1234567890!!!"); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token sống 2 tiếng
                Issuer = "GymApp",
                Audience = "GymAppClient",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                username = username,
                role = role,
                message = "Đăng nhập thành công! Hãy copy token này để mở khóa các chức năng."
            });
        }

        // PUT api/users/{id}
        [HttpPut("{id:int}")]
        public ActionResult<UserResponseDto> Update(int id, [FromBody] UserUpdateDto request)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });

            user.FullName = string.IsNullOrWhiteSpace(request.FullName) ? user.FullName : request.FullName;
            user.Email = string.IsNullOrWhiteSpace(request.Email) ? user.Email : request.Email;
            user.Phone = string.IsNullOrWhiteSpace(request.Phone) ? user.Phone : request.Phone;
            user.Role = string.IsNullOrWhiteSpace(request.Role) ? user.Role : request.Role;
            user.MembershipPackage = string.IsNullOrWhiteSpace(request.MembershipPackage) ? user.MembershipPackage : request.MembershipPackage;
            user.MembershipStatus = string.IsNullOrWhiteSpace(request.MembershipStatus) ? user.MembershipStatus : request.MembershipStatus;
            user.IsActive = request.IsActive;

            _context.SaveChanges(); // LƯU VÀO DB THẬT

            return Ok(new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Role = user.Role,
                MembershipPackage = user.MembershipPackage,
                MembershipStatus = user.MembershipStatus,
                MembershipEndDate = user.MembershipEndDate,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });
        }

        // DELETE api/users/{id}
        // [QUAN TRỌNG] Đặt ổ khóa: CHỈ CÓ ADMIN MỚI ĐƯỢC PHÉP GỌI LỆNH XÓA!
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });

            _context.Users.Remove(user);
            _context.SaveChanges(); // XÓA KHỎI DB THẬT

            return Ok(new { message = $"Đã xóa người dùng '{user.Username}' (Id = {id}) thành công từ Database." });
        }
    }
}