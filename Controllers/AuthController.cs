using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Xac thuc nguoi dung (Login)
    /// Ap dung mo hinh 3 lop: Goi thong qua IUserService
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // ------------------------------------------------------------------------
        // LOGIN (POST) - api/auth/login
        // ------------------------------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            var result = await _userService.LoginAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                token     = result.Token,
                username  = result.Username,
                role      = result.Role,
                expiresAt = DateTime.UtcNow.AddHours(2),
                message   = result.Message
            });
        }
    }
}
