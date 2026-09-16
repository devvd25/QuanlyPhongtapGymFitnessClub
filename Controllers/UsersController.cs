using Buoi2_WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Buoi2_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Mock Data
        private static List<UserResponseDto> _fakeUsers = new List<UserResponseDto>
        {
            new UserResponseDto
            {
                Id = 1,
                Username = "nguyenvana",
                Email = "ana@gmail.com",
                FullName = "Nguyễn Văn A",
                Phone = "0901234567",
                Role = "Member",
                MembershipPackage = "VIP",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2027, 12, 31),
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 15)
            },
            new UserResponseDto
            {
                Id = 2,
                Username = "tranvanb",
                Email = "banb@gmail.com",
                FullName = "Trần Văn B",
                Phone = "0912345678",
                Role = "Member",
                MembershipPackage = "Basic",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2027, 6, 30),
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 20)
            },
            new UserResponseDto
            {
                Id = 3,
                Username = "lethic",
                Email = "chicle@gmail.com",
                FullName = "Lê Thị C",
                Phone = "0923456789",
                Role = "Trainer",
                MembershipPackage = "Diamond",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2028, 1, 1),
                IsActive = true,
                CreatedAt = new DateTime(2025, 11, 5)
            },
            new UserResponseDto
            {
                Id = 4,
                Username = "phamvand",
                Email = "d.pham@gmail.com",
                FullName = "Phạm Văn D",
                Phone = "0934567890",
                Role = "Admin",
                MembershipPackage = "Diamond",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2029, 1, 1),
                IsActive = true,
                CreatedAt = new DateTime(2025, 8, 10)
            },
            new UserResponseDto
            {
                Id = 5,
                Username = "hoangthie",
                Email = "ehoang@gmail.com",
                FullName = "Hoàng Thị E",
                Phone = "0945678901",
                Role = "Member",
                MembershipPackage = "VIP",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2027, 8, 15),
                IsActive = true,
                CreatedAt = new DateTime(2026, 2, 14)
            },
            new UserResponseDto
            {
                Id = 6,
                Username = "vovanf",
                Email = "fvovo@gmail.com",
                FullName = "Võ Văn F",
                Phone = "0956789012",
                Role = "Member",
                MembershipPackage = "Basic",
                MembershipStatus = "Expired",
                MembershipEndDate = new DateTime(2026, 1, 1),
                IsActive = false,
                CreatedAt = new DateTime(2025, 6, 1)
            },
            new UserResponseDto
            {
                Id = 7,
                Username = "dangthig",
                Email = "gdang@gmail.com",
                FullName = "Đặng Thị G",
                Phone = "0967890123",
                Role = "Trainer",
                MembershipPackage = "Diamond",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2028, 5, 20),
                IsActive = true,
                CreatedAt = new DateTime(2025, 10, 12)
            },
            new UserResponseDto
            {
                Id = 8,
                Username = "buivanh",
                Email = "hbuivan@gmail.com",
                FullName = "Bùi Văn H",
                Phone = "0978901234",
                Role = "Member",
                MembershipPackage = "VIP",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2027, 11, 30),
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 5)
            },
            new UserResponseDto
            {
                Id = 9,
                Username = "ngothii",
                Email = "ingothi@gmail.com",
                FullName = "Ngô Thị I",
                Phone = "0989012345",
                Role = "Member",
                MembershipPackage = "Basic",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2027, 4, 25),
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 18)
            },
            new UserResponseDto
            {
                Id = 10,
                Username = "dinhvank",
                Email = "kdinh@gmail.com",
                FullName = "Đinh Văn K",
                Phone = "0990123456",
                Role = "Member",
                MembershipPackage = "Diamond",
                MembershipStatus = "Active",
                MembershipEndDate = new DateTime(2028, 2, 28),
                IsActive = true,
                CreatedAt = new DateTime(2026, 6, 10)
            }
        };

        private static readonly string _adminUsername = "admin";
        private static readonly string _adminPassword = "123456";

        // GET api/users
        [HttpGet]
        public ActionResult<List<UserResponseDto>> GetAll()
        {
            return Ok(_fakeUsers);
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public ActionResult<UserResponseDto> GetById(int id)
        {
            var user = _fakeUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });

            return Ok(user);
        }

        // POST api/users/register
        [HttpPost("register")]
        public ActionResult<UserResponseDto> Register([FromBody] UserRegisterDto request)
        {
            if (_fakeUsers.Any(u => u.Username == request.Username))
                return BadRequest(new { message = $"Username '{request.Username}' đã tồn tại." });

            if (_fakeUsers.Any(u => u.Email == request.Email))
                return BadRequest(new { message = $"Email '{request.Email}' đã được đăng ký." });

            var newUser = new UserResponseDto
            {
                Id = _fakeUsers.Max(u => u.Id) + 1,
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                Phone = request.Phone,
                Role = "Member",
                MembershipPackage = request.MembershipPackage,
                MembershipStatus = "Active",
                MembershipEndDate = DateTime.UtcNow.AddMonths(1),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _fakeUsers.Add(newUser);

            // 201 Created + trả về URL của resource vừa tạo
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        // POST api/users/login
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDto request)
        {
            if (request.Username == _adminUsername && request.Password == _adminPassword)
            {
                return Ok(new
                {
                    token = "fake-jwt-token-xyz-123",
                    username = request.Username,
                    role = "Admin",
                    message = "Đăng nhập thành công!"
                });
            }

            // Kiểm tra trong list user, password mặc định = "123456"
            var user = _fakeUsers.FirstOrDefault(u => u.Username == request.Username);
            if (user != null && request.Password == "123456")
            {
                return Ok(new
                {
                    token = $"fake-jwt-token-{user.Username}-{user.Id}",
                    username = user.Username,
                    role = user.Role,
                    message = "Đăng nhập thành công!"
                });
            }

            return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
        }

        // PUT api/users/{id}
        [HttpPut("{id:int}")]
        public ActionResult<UserResponseDto> Update(int id, [FromBody] UserUpdateDto request)
        {
            var user = _fakeUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });

            user.FullName = request.FullName;
            user.Phone = request.Phone;

            return Ok(user);
        }

        // DELETE api/users/{id}
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var user = _fakeUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = $"Không tìm thấy người dùng có Id = {id}" });

            _fakeUsers.Remove(user);

            return Ok(new { message = $"Đã xóa người dùng '{user.Username}' (Id = {id}) thành công." });
        }

        // GET api/users/search?name=nguyen
        [HttpGet("search")]
        public ActionResult SearchByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Vui lòng nhập tên cần tìm." });

            var results = _fakeUsers
                .Where(u => u.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)
                         || u.Username.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(new
            {
                keyword = name,
                totalFound = results.Count,
                users = results
            });
        }

        // GET api/users/stats - Thống kê theo Role và gói tập
        [HttpGet("stats")]
        public ActionResult GetStats()
        {
            var statsByRole = _fakeUsers
                .GroupBy(u => u.Role)
                .Select(g => new { role = g.Key, count = g.Count() })
                .ToList();

            var statsByPackage = _fakeUsers
                .GroupBy(u => u.MembershipPackage)
                .Select(g => new { package_ = g.Key, count = g.Count() })
                .ToList();

            return Ok(new
            {
                totalUsers = _fakeUsers.Count,
                byRole = statsByRole,
                byMembershipPackage = statsByPackage
            });
        }
    }
}