using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;
using Microsoft.AspNetCore.Mvc;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quản lý Nhân viên lễ tân và Bán hàng
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="search">Tìm theo tên, username hoặc phòng ban</param>
        /// <param name="workShift">Lọc theo ca làm việc</param>
        /// <param name="isOnDuty">Lọc theo trạng thái đang trực</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<StaffResponseDto>), StatusCodes.Status200OK)]
        public ActionResult<List<StaffResponseDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? workShift,
            [FromQuery] bool? isOnDuty)
        {
            var staffs = _staffService.GetAll(search, workShift, isOnDuty);
            return Ok(staffs);
        }

        /// <summary>
        /// Lấy thông tin nhân viên theo ID
        /// </summary>
        /// <param name="id">ID nhân viên</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StaffResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StaffResponseDto> GetById([FromRoute] int id)
        {
            var staff = _staffService.GetById(id);
            if (staff == null)
            {
                return NotFound(new { message = $"Không tìm thấy nhân viên có Id = {id}" });
            }
            return Ok(staff);
        }

        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        /// <param name="dto">Thông tin nhân viên</param>
        [HttpPost]
        [ProducesResponseType(typeof(StaffResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StaffResponseDto> Create([FromBody] StaffCreateDto dto)
        {
            var created = _staffService.Create(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="id">ID nhân viên</param>
        /// <param name="dto">Dữ liệu cập nhật</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromRoute] int id, [FromBody] StaffUpdateDto dto)
        {
            var updated = _staffService.Update(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"Không tìm thấy nhân viên có Id = {id} để cập nhật" });
            }

            return NoContent();
        }

        /// <summary>
        /// Xóa nhân viên theo ID
        /// </summary>
        /// <param name="id">ID nhân viên</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            bool isDeleted = _staffService.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"Không tìm thấy nhân viên có Id = {id} để xóa" });
            }

            return NoContent();
        }

        /// <summary>
        /// Đổi trạng thái trực ca của nhân viên
        /// </summary>
        /// <param name="id">ID nhân viên</param>
        [HttpPatch("{id:int}/toggle-duty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ToggleDutyStatus([FromRoute] int id)
        {
            bool success = _staffService.ToggleDutyStatus(id);
            if (!success)
            {
                return NotFound(new { message = $"Không tìm thấy nhân viên có Id = {id}" });
            }

            var staff = _staffService.GetById(id);
            return Ok(new
            {
                message = $"Đã cập nhật trạng thái trực ca của nhân viên {staff?.FullName} thành công.",
                isOnDuty = staff?.IsOnDuty
            });
        }
    }
}
