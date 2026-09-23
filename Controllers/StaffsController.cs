using Buoi2_WebAPI.DTOs;
using Buoi2_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buoi2_WebAPI.Controllers
{
    /// <summary>
    /// API Quản lý Nhân viên lễ tân và Bán hàng (Front Desk Staff)
    /// Áp dụng đầy đủ: Dependency Injection (Buổi 2), RESTful CRUD (Buổi 3), DTOs và Validation (Buổi 4)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        /// <summary>
        /// Khởi tạo StaffsController với Dependency Injection của IStaffService
        /// </summary>
        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        /// <summary>
        /// Lấy danh sách tất cả nhân viên phòng gym (hỗ trợ lọc theo ca làm việc và trạng thái trực)
        /// </summary>
        /// <param name="search">Tìm kiếm theo họ tên, username hoặc phòng ban</param>
        /// <param name="workShift">Lọc theo ca trực: Morning, Afternoon, Evening, FullDay</param>
        /// <param name="isOnDuty">Lọc theo trạng thái đang trực quầy (true: đang trực, false: nghỉ ca)</param>
        /// <returns>Danh sách nhân viên</returns>
        /// <response code="200">Lấy danh sách thành công</response>
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
        /// <param name="id">ID của nhân viên</param>
        /// <returns>Thông tin nhân viên</returns>
        /// <response code="200">Tìm thấy nhân viên</response>
        /// <response code="404">Không tìm thấy nhân viên với ID chỉ định</response>
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
        /// Thêm mới một nhân viên lễ tân vào hệ thống
        /// </summary>
        /// <remarks>
        /// Dữ liệu đầu vào bắt buộc:
        /// - Độ tuổi hợp lệ từ 18 đến 65 (Custom Validation [GymAgeValidation])
        /// - Ca làm việc: Morning, Afternoon, Evening, FullDay
        /// - Email, SĐT đúng định dạng chuẩn
        /// </remarks>
        /// <param name="dto">Thông tin nhân viên cần tạo</param>
        /// <returns>Nhân viên vừa tạo kèm Location header</returns>
        /// <response code="201">Tạo mới thành công</response>
        /// <response code="400">Dữ liệu vi phạm quy tắc validation</response>
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
        /// Cập nhật thông tin nhân viên theo ID
        /// </summary>
        /// <param name="id">ID nhân viên cần cập nhật</param>
        /// <param name="dto">Dữ liệu cập nhật</param>
        /// <returns>Không có nội dung trả về nếu cập nhật thành công (204 NoContent)</returns>
        /// <response code="204">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu cập nhật không hợp lệ</response>
        /// <response code="404">Không tìm thấy nhân viên để cập nhật</response>
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
        /// Xóa một nhân viên khỏi hệ thống theo ID
        /// </summary>
        /// <param name="id">ID nhân viên cần xóa</param>
        /// <returns>Không có nội dung trả về nếu xóa thành công (204 NoContent)</returns>
        /// <response code="204">Xóa thành công</response>
        /// <response code="404">Không tìm thấy nhân viên để xóa</response>
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
        /// Đổi trạng thái trực ca của nhân viên (Đang trực quầy / Nghỉ ca)
        /// </summary>
        /// <param name="id">ID của nhân viên</param>
        /// <response code="200">Đổi trạng thái trực ca thành công</response>
        /// <response code="404">Không tìm thấy nhân viên</response>
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
