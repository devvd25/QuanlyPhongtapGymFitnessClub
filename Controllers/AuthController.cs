using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Xac thuc nguoi dung (Login)
    /// Ap dung mo hinh 3 lop: Controller -> IAuthService -> IAuthRepository
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ------------------------------------------------------------------------
        // LOGIN (POST) - api/auth/login
        // ------------------------------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                return BadRequest(new { message = "Ten dang nhap hoac mat khau khong chinh xac." });
            }

            return Ok(new
            {
                token     = token,
                username  = request.Username,
                expiresAt = DateTime.UtcNow.AddMinutes(60),
                message   = "Dang nhap thanh cong!"
            });
        }

        // ------------------------------------------------------------------------
        // GET CURRENT USER (GET) - api/auth/me (Yêu cầu phải có Token)
        // ------------------------------------------------------------------------
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                userId,
                username,
                role,
                message = "Xác thực JWT Token thành công!"
            });
        }
    }
}
