using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        // 1. Khai báo biến DbContext
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // 2. Inject AppDbContext và IConfiguration thông qua Constructor
        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private static readonly string _adminUsername = "admin";
        private static readonly string _adminPassword = "password123";

        // ------------------------------------------------------------------------
        // READ ALL (GET) - Lấy danh sách hội viên/người dùng (hỗ trợ lọc & tìm kiếm)
        // Áp dụng Best Practice Buổi 6: async/await với ToListAsync()
        // ------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? role,
            [FromQuery] string? package)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.Trim().ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(searchLower) || 
                                         u.Username.ToLower().Contains(searchLower) ||
                                         u.Phone.Contains(searchLower));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(u => u.Role == role);
            }

            if (!string.IsNullOrWhiteSpace(package))
            {
                query = query.Where(u => u.MembershipPackage == package);
            }

            // Dùng LINQ .Select() để ánh xạ sang DTO (tránh lộ trường mật khẩu)
            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Role = u.Role,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    MembershipPackage = u.MembershipPackage,
                    MembershipStatus = u.MembershipStatus,
                    MembershipEndDate = u.MembershipEndDate,
                    RemainingPtSessions = u.RemainingPtSessions,
                    AssignedTrainerId = u.AssignedTrainerId,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        // ------------------------------------------------------------------------
        // READ BY ID (GET) - Lấy thông tin người dùng theo Id
        // ------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Role = u.Role,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    MembershipPackage = u.MembershipPackage,
                    MembershipStatus = u.MembershipStatus,
                    MembershipEndDate = u.MembershipEndDate,
                    RemainingPtSessions = u.RemainingPtSessions,
                    AssignedTrainerId = u.AssignedTrainerId,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });
            }

            return Ok(user);
        }

        // ------------------------------------------------------------------------
        // CREATE / REGISTER (POST) - Đăng ký hội viên / Thêm người dùng mới
        // ------------------------------------------------------------------------
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegisterDto request)
        {
            // Kiểm tra trùng lặp Username hoặc Email
            bool isExist = await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (isExist)
            {
                return BadRequest(new { message = "Username hoặc Email đã được sử dụng." });
            }

            // Băm mật khẩu bảo mật bằng HMACSHA512
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FullName = request.FullName,
                Phone = request.Phone,
                Gender = string.IsNullOrEmpty(request.Gender) ? "Male" : request.Gender,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address ?? string.Empty,
                HeightCm = request.HeightCm,
                WeightKg = request.WeightKg,
                HealthNotes = request.HealthNotes,
                FitnessGoal = request.FitnessGoal,
                Role = string.IsNullOrEmpty(request.Role) ? "Member" : request.Role,
                MembershipPackage = string.IsNullOrEmpty(request.MembershipPackage) ? "Basic" : request.MembershipPackage,
                MembershipStatus = "Active",
                MembershipStartDate = DateTime.UtcNow,
                MembershipEndDate = DateTime.UtcNow.AddMonths(1),
                RemainingPtSessions = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var response = MapToDto(newUser);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // ------------------------------------------------------------------------
        // UPDATE (PUT) - Cập nhật thông tin người dùng (Có thể đổi mật khẩu)
        // ------------------------------------------------------------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });
            }

            // Cập nhật các trường thông tin nếu có gửi lên
            if (!string.IsNullOrEmpty(request.Username)) user.Username = request.Username;
            if (!string.IsNullOrEmpty(request.Email)) user.Email = request.Email;
            if (!string.IsNullOrEmpty(request.FullName)) user.FullName = request.FullName;
            if (!string.IsNullOrEmpty(request.Phone)) user.Phone = request.Phone;
            if (!string.IsNullOrEmpty(request.Role)) user.Role = request.Role;
            if (!string.IsNullOrEmpty(request.MembershipPackage)) user.MembershipPackage = request.MembershipPackage;
            if (!string.IsNullOrEmpty(request.MembershipStatus)) user.MembershipStatus = request.MembershipStatus;
            if (!string.IsNullOrEmpty(request.Gender)) user.Gender = request.Gender;
            if (request.DateOfBirth.HasValue) user.DateOfBirth = request.DateOfBirth;
            if (request.Address != null) user.Address = request.Address;
            if (request.RemainingPtSessions.HasValue) user.RemainingPtSessions = request.RemainingPtSessions.Value;
            if (request.AssignedTrainerId.HasValue) user.AssignedTrainerId = request.AssignedTrainerId.Value;
            if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;

            // Nếu có nhập mật khẩu mới thì băm lại mật khẩu
            if (!string.IsNullOrEmpty(request.Password))
            {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Cập nhật thành công người dùng Id = {id}",
                user = MapToDto(user)
            });
        }

        // ------------------------------------------------------------------------
        // DELETE (DELETE) - Xóa người dùng theo Id
        // ------------------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã xóa người dùng '{user.Username}' (Id = {id}) thành công" });
        }

        // ------------------------------------------------------------------------
        // AUTHENTICATION (POST) - Đăng nhập nhận JWT Token (Kiểm tra Hash thực tế)
        // ------------------------------------------------------------------------
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto request)
        {
            string role;
            string username;

            // Tài khoản admin hệ thống mặc định
            if (request.Username == _adminUsername && request.Password == _adminPassword)
            {
                role = "Admin";
                username = _adminUsername;
            }
            else
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                // Kiểm tra tài khoản và mật khẩu đã băm trong database
                if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
                }

                if (!user.IsActive)
                {
                    return BadRequest(new { message = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên." });
                }

                role = user.Role;
                username = user.Username;

                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            // Tạo JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "MySuperSecretKeyForGymApp_1234567890!!!";
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = jwtSettings["Issuer"] ?? "GymApp",
                Audience = jwtSettings["Audience"] ?? "GymAppClient",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                username = username,
                role = role,
                message = "Đăng nhập thành công!"
            });
        }

        // ------------------------------------------------------------------------
        // DASHBOARD STATS (GET) - Thống kê hội viên phòng Gym
        // ------------------------------------------------------------------------
        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var totalUsers = await _context.Users.CountAsync();
            var byRole = await _context.Users
                .GroupBy(u => u.Role)
                .Select(g => new { role = g.Key, count = g.Count() })
                .ToListAsync();

            var byPackage = await _context.Users
                .GroupBy(u => u.MembershipPackage)
                .Select(g => new { package = g.Key, count = g.Count() })
                .ToListAsync();

            return Ok(new
            {
                totalUsers,
                statsByRole = byRole,
                statsByMembershipPackage = byPackage
            });
        }

        // ========================================================================
        // Helper Methods: Băm mật khẩu & Ánh xạ DTO
        // ========================================================================
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (passwordSalt == null || passwordSalt.Length == 0 || passwordHash == null || passwordHash.Length == 0)
                return false;

            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private static UserResponseDto MapToDto(User u)
        {
            return new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                Role = u.Role,
                Gender = u.Gender,
                DateOfBirth = u.DateOfBirth,
                Address = u.Address,
                MembershipPackage = u.MembershipPackage,
                MembershipStatus = u.MembershipStatus,
                MembershipEndDate = u.MembershipEndDate,
                RemainingPtSessions = u.RemainingPtSessions,
                AssignedTrainerId = u.AssignedTrainerId,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            };
        }
    }
}