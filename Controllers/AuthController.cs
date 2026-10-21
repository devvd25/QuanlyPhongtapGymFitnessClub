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
    }
}
