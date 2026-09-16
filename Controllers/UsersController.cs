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
                Username = "nguyenvanan",
                Email = "an.nguyen@gmail.com",
                FullName = "Nguyễn Văn An",
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
                Username = "tranvanbinh",
                Email = "binh.tran@gmail.com",
                FullName = "Trần Văn Bình",
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
                Username = "lethicam",
                Email = "cam.le@gmail.com",
                FullName = "Lê Thị Cẩm",
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
                Username = "phamvandung",
                Email = "dung.pham@gmail.com",
                FullName = "Phạm Văn Dũng",
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
                Username = "hoangthimai",
                Email = "mai.hoang@gmail.com",
                FullName = "Hoàng Thị Mai",
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
                Username = "vovanphuc",
                Email = "phuc.vo@gmail.com",
                FullName = "Võ Văn Phúc",
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
                Username = "dangthigiang",
                Email = "giang.dang@gmail.com",
                FullName = "Đặng Thị Giang",
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
                Username = "buivanhuy",
                Email = "huy.bui@gmail.com",
                FullName = "Bùi Văn Huy",
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
                Username = "ngothiyen",
                Email = "yen.ngo@gmail.com",
                FullName = "Ngô Thị Yến",
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
                Username = "dinhvankhoa",
                Email = "khoa.dinh@gmail.com",
                FullName = "Đinh Văn Vũ",
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

        // GET api/users?search=...&role=...&package=...
        [HttpGet]
        public ActionResult GetAll(
            [FromQuery] string? search,
            [FromQuery] UserRole? role,
            [FromQuery] MembershipPackageType? package)
        {
            var query = _fakeUsers.AsEnumerable();
            bool hasFilter = false;

            // 1. Lọc theo từ khóa tìm kiếm (Tên hoặc Username)
            if (!string.IsNullOrWhiteSpace(search))
            {
                hasFilter = true;
                query = query.Where(u => u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)
                                      || u.Username.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Lọc theo Role (Admin, Member, Trainer...)
            if (role.HasValue)
            {
                hasFilter = true;
                string roleStr = role.Value.ToString();
                query = query.Where(u => u.Role.Equals(roleStr, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Lọc theo Gói tập Gym (Basic, VIP, Diamond...)
            if (package.HasValue)
            {
                hasFilter = true;
                string packageStr = package.Value.ToString();
                query = query.Where(u => u.MembershipPackage.Equals(packageStr, StringComparison.OrdinalIgnoreCase));
            }

            var matchedUsers = query.ToList();

            // Khi có sử dụng bất kỳ bộ lọc nào -> Chỉ trả về kết quả lọc (không hiện stats)
            if (hasFilter)
            {
                return Ok(new
                {
                    filters = new
                    {
                        search,
                        role = role?.ToString(),
                        package = package?.ToString()
                    },
                    totalFound = matchedUsers.Count,
                    users = matchedUsers
                });
            }

            // Khi không lọc (lấy tất cả) -> Trả về toàn bộ danh sách kèm thống kê
            var statsByRole = _fakeUsers
                .GroupBy(u => u.Role)
                .Select(g => new { role = g.Key, count = g.Count() })
                .ToList();

            var statsByPackage = _fakeUsers
                .GroupBy(u => u.MembershipPackage)
                .Select(g => new { package = g.Key, count = g.Count() })
                .ToList();

            return Ok(new
            {
                totalSystemUsers = _fakeUsers.Count,
                stats = new
                {
                    byRole = statsByRole,
                    byMembershipPackage = statsByPackage
                },
                users = _fakeUsers
            });
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

            user.FullName = string.IsNullOrWhiteSpace(request.FullName) ? user.FullName : request.FullName;
            user.Email = string.IsNullOrWhiteSpace(request.Email) ? user.Email : request.Email;
            user.Phone = string.IsNullOrWhiteSpace(request.Phone) ? user.Phone : request.Phone;
            user.Role = string.IsNullOrWhiteSpace(request.Role) ? user.Role : request.Role;
            user.MembershipPackage = string.IsNullOrWhiteSpace(request.MembershipPackage) ? user.MembershipPackage : request.MembershipPackage;
            user.MembershipStatus = string.IsNullOrWhiteSpace(request.MembershipStatus) ? user.MembershipStatus : request.MembershipStatus;
            user.IsActive = request.IsActive;

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
    }
}