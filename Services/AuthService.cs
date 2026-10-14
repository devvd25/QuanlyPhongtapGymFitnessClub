using Microsoft.IdentityModel.Tokens;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;
using QuanlyPhongtapGymFitnessClub.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private const string _adminUsername = "admin";
        private const string _adminPassword = "Admin@123";

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            // 1. Kiem tra tai khoan Admin quan tri vien
            if (dto.Username == _adminUsername && dto.Password == _adminPassword)
            {
                return GenerateJwtToken(0, _adminUsername, "Admin");
            }

            // 2. Tim kiem nguoi dung trong Database thong qua AuthRepository
            var user = await _authRepository.GetByUsernameAsync(dto.Username);

            if (user == null || !user.IsActive)
            {
                return null; // Khong tim thay user hoac tai khoan da bi khoa
            }

            // 3. Su dung lai PasswordSalt tu database cua user do de bam lai mat khau nguoi dung vua nhap
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            // So sanh tung byte cua mang computedHash voi user.PasswordHash trong database
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return null; // Mat khau khong chinh xac
                }
            }

            // 4. Dang nhap thanh cong, tien hanh tao token cho Hoi vien (Member)
            return GenerateJwtToken(user.Id, user.Username, "Member");
        }

        private string GenerateJwtToken(int userId, string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "Scretkey123456789123456789123456789123456789");

            // Khai bao cac Claims cho Token (Phuc vu cho viec phan quyen Authorization)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(secretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = double.TryParse(jwtSettings["ExpiryMinutes"], out double exp) ? exp : 60;

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "https://localhost:7003",
                audience: jwtSettings["Audience"] ?? "https://localhost:7003",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
