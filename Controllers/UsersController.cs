using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quan ly Hoi vien Gym (Users)
    /// Su dung mo hinh 3 lop: Controller -> Service -> Repository
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        // Inject IUserService thay vi AppDbContext
        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? membershipStatus)
        {
            var users = await _userService.GetAllAsync(search, membershipStatus);
            return Ok(users);
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"Khong tim thay nguoi dung co Id = {id}" });
            }

            return Ok(user);
        }

        // POST api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegisterDto request)
        {
            var result = await _userService.RegisterAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        // POST api/users/login (Su dung IAuthService theo dung code cua Thay)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (token == null)
            {
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
            }

            return Ok(new { Token = token });
        }

        // PUT api/users/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto request)
        {
            var result = await _userService.UpdateAsync(id, request);
            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        // DELETE api/users/{id} (Yeu cau quyen Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
