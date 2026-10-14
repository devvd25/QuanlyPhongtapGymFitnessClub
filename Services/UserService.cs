using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;
using QuanlyPhongtapGymFitnessClub.Repositories;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    /// <summary>
    /// Lop Service xu ly logic nghiep vu cho Hoi vien (Users)
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private const string _adminUsername = "admin";
        private const string _adminPassword = "Admin@123";

        public UserService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<List<UserResponseDto>> GetAllAsync(string? search = null, string? membershipStatus = null)
        {
            var users = await _userRepo.GetAllAsync(search, membershipStatus);
            return users.Select(MapToResponseDto).ToList();
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return user == null ? null : MapToResponseDto(user);
        }

        public async Task<(bool IsSuccess, string Message, UserResponseDto? Data)> RegisterAsync(UserRegisterDto request)
        {
            var existingUser = await _userRepo.GetByUsernameOrEmailAsync(request.Username, request.Email);
            if (existingUser != null)
            {
                return (false, "Username hoac Email da duoc su dung.", null);
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Username            = request.Username,
                Email               = request.Email,
                PasswordHash        = passwordHash,
                PasswordSalt        = passwordSalt,
                FullName            = request.FullName,
                Phone               = request.Phone,
                Gender              = request.Gender ?? "Male",
                DateOfBirth         = request.DateOfBirth,
                Address             = request.Address ?? string.Empty,
                HeightCm            = request.HeightCm,
                WeightKg            = request.WeightKg,
                HealthNotes         = request.HealthNotes,
                FitnessGoal         = request.FitnessGoal,
                MembershipPackage   = request.MembershipPackage ?? "Basic",
                MembershipStatus    = "Active",
                MembershipStartDate = DateTime.UtcNow,
                MembershipEndDate   = DateTime.UtcNow.AddMonths(1),
                IsActive            = true,
                IsDeleted           = false,
                CreatedAt           = DateTime.UtcNow
            };

            await _userRepo.AddAsync(newUser);
            await _userRepo.SaveChangesAsync();

            return (true, "Dang ky thanh cong!", MapToResponseDto(newUser));
        }

        public async Task<(bool IsSuccess, string Message, string? Token, string? Username, string? Role)> LoginAsync(UserLoginDto request)
        {
            string role = string.Empty;
            string username = string.Empty;

            if (request.Username == _adminUsername && request.Password == _adminPassword)
            {
                role = "Admin";
                username = _adminUsername;
            }
            else
            {
                var user = await _userRepo.GetByUsernameOrEmailAsync(request.Username, request.Username);
                if (user == null)
                {
                    return (false, "Ten dang nhap hoac mat khau khong chinh xac.", null, null, null);
                }

                bool isValid = VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
                if (!isValid)
                {
                    return (false, "Ten dang nhap hoac mat khau khong chinh xac.", null, null, null);
                }

                if (!user.IsActive)
                {
                    return (false, "Tai khoan da bi khoa. Vui long lien he quan tri vien.", null, null, null);
                }

                role = "Member";
                username = user.Username;

                user.LastLoginAt = DateTime.UtcNow;
                _userRepo.Update(user);
                await _userRepo.SaveChangesAsync();
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSettings  = _config.GetSection("JwtSettings");
            var secretKey    = jwtSettings["SecretKey"] ?? "Scretkey123456789123456789123456789123456789";
            var key          = Encoding.UTF8.GetBytes(secretKey);
            var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out int exp) ? exp : 60;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires            = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer             = jwtSettings["Issuer"] ?? "https://localhost:7003",
                Audience           = jwtSettings["Audience"] ?? "https://localhost:7003",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (true, "Dang nhap thanh cong!", tokenString, username, role);
        }

        public async Task<(bool IsSuccess, string Message)> UpdateAsync(int id, UserUpdateDto request)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return (false, $"Khong tim thay nguoi dung co Id = {id}");
            }

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                var existing = await _userRepo.GetByUsernameOrEmailAsync(string.Empty, request.Email);
                if (existing != null && existing.Id != id)
                {
                    return (false, "Email da duoc su dung boi nguoi dung khac.");
                }
                user.Email = request.Email;
            }

            if (!string.IsNullOrWhiteSpace(request.FullName))          user.FullName          = request.FullName;
            if (!string.IsNullOrWhiteSpace(request.Phone))             user.Phone             = request.Phone;
            if (!string.IsNullOrWhiteSpace(request.MembershipPackage)) user.MembershipPackage = request.MembershipPackage;
            if (!string.IsNullOrWhiteSpace(request.MembershipStatus))  user.MembershipStatus  = request.MembershipStatus;
            if (request.IsActive.HasValue)                             user.IsActive          = request.IsActive.Value;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

            return (true, $"Cap nhat thanh cong nguoi dung Id = {id}");
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return (false, $"Khong tim thay nguoi dung co Id = {id}");
            }

            _userRepo.Delete(user);
            await _userRepo.SaveChangesAsync();

            return (true, $"Da xoa nguoi dung Id = {id}");
        }

        // ================= HELPER METHODS =================
        private static UserResponseDto MapToResponseDto(User user) => new()
        {
            Id                = user.Id,
            Username          = user.Username,
            Email             = user.Email,
            FullName          = user.FullName,
            Phone             = user.Phone,
            MembershipPackage = user.MembershipPackage,
            MembershipStatus  = user.MembershipStatus,
            MembershipEndDate = user.MembershipEndDate,
            IsActive          = user.IsActive,
            CreatedAt         = user.CreatedAt
        };

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).SequenceEqual(storedHash);
        }
    }
}

