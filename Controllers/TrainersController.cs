using Buoi2_WebAPI.DTOs;
using Buoi2_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Buoi2_WebAPI.Controllers
{
    /// <summary>
    /// API Quản lý Huấn luyện viên cá nhân (Personal Trainers)
    /// Áp dụng đầy đủ: Dependency Injection (Buổi 2), RESTful CRUD và Nested Route (Buổi 3), DTOs và Validation (Buổi 4)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        /// <summary>
        /// Tiêm ITrainerService vào Controller thông qua Constructor (Buổi 2: Dependency Injection)
        /// </summary>
        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        /// <summary>
        /// Lấy danh sách tất cả Huấn luyện viên (hỗ trợ tìm kiếm và lọc dữ liệu)
        /// </summary>
        /// <param name="search">Từ khóa tìm kiếm theo tên, username hoặc chuyên môn</param>
        /// <param name="specialty">Lọc theo chuyên môn (Gym, Fitness, Bodybuilding, Yoga, Boxing, Pilates)</param>
        /// <param name="minExperienceYears">Lọc theo số năm kinh nghiệm tối thiểu</param>
        /// <returns>Danh sách Huấn luyện viên phù hợp</returns>
        /// <response code="200">Lấy danh sách thành công</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TrainerResponseDto>), StatusCodes.Status200OK)]
        public ActionResult<List<TrainerResponseDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? specialty,
            [FromQuery] int? minExperienceYears)
        {
            var trainers = _trainerService.GetAll(search, specialty, minExperienceYears);
            return Ok(trainers);
        }

        /// <summary>
        /// Lấy thông tin chi tiết một Huấn luyện viên theo ID
        /// </summary>
        /// <param name="id">ID của Huấn luyện viên (phải là số nguyên)</param>
        /// <returns>Thông tin Huấn luyện viên</returns>
        /// <response code="200">Tìm thấy Huấn luyện viên</response>
        /// <response code="404">Không tìm thấy Huấn luyện viên với ID chỉ định</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TrainerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TrainerResponseDto> GetById([FromRoute] int id)
        {
            var trainer = _trainerService.GetById(id);
            if (trainer == null)
            {
                return NotFound(new { message = $"Không tìm thấy Huấn luyện viên có Id = {id}" });
            }
            return Ok(trainer);
        }

        /// <summary>
        /// Thêm mới một Huấn luyện viên vào phòng gym
        /// </summary>
        /// <remarks>
        /// Dữ liệu đầu vào sẽ được tự động kiểm tra tính hợp lệ:
        /// - Độ tuổi phải từ 18 đến 60 (Custom Validation [GymAgeValidation])
        /// - Số lượng học viên tối đa phải là số chẵn (Custom Validation [MustBeEven])
        /// - Email, SĐT, Username theo đúng định dạng chuẩn
        /// </remarks>
        /// <param name="dto">Thông tin Huấn luyện viên cần tạo mới</param>
        /// <returns>Huấn luyện viên vừa được tạo kèm URL Location</returns>
        /// <response code="201">Tạo mới thành công, trả về Location header và dữ liệu vừa tạo</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ hoặc vi phạm quy tắc validation</response>
        [HttpPost]
        [ProducesResponseType(typeof(TrainerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TrainerResponseDto> Create([FromBody] TrainerCreateDto dto)
        {
            var created = _trainerService.Create(dto);

            // Trả về 201 Created kèm Header Location trỏ tới endpoint GET /api/trainers/{id}
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Cập nhật thông tin Huấn luyện viên theo ID
        /// </summary>
        /// <param name="id">ID của Huấn luyện viên cần cập nhật</param>
        /// <param name="dto">Dữ liệu cập nhật mới</param>
        /// <returns>Không có nội dung trả về nếu cập nhật thành công (204 NoContent)</returns>
        /// <response code="204">Cập nhật thành công (No Content)</response>
        /// <response code="400">Dữ liệu cập nhật không hợp lệ</response>
        /// <response code="404">Không tìm thấy Huấn luyện viên để cập nhật</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromRoute] int id, [FromBody] TrainerUpdateDto dto)
        {
            var updated = _trainerService.Update(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"Không tìm thấy Huấn luyện viên có Id = {id} để cập nhật" });
            }

            return NoContent(); // 204 NoContent chuẩn RESTful
        }

        /// <summary>
        /// Xóa một Huấn luyện viên khỏi hệ thống theo ID
        /// </summary>
        /// <param name="id">ID của Huấn luyện viên cần xóa</param>
        /// <returns>Không có nội dung trả về nếu xóa thành công (204 NoContent)</returns>
        /// <response code="204">Xóa thành công (No Content)</response>
        /// <response code="404">Không tìm thấy Huấn luyện viên để xóa</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            bool isDeleted = _trainerService.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"Không tìm thấy Huấn luyện viên có Id = {id} để xóa" });
            }

            return NoContent(); // 204 NoContent chuẩn RESTful
        }

        /// <summary>
        /// Nested Route: Lấy danh sách hội viên do Huấn luyện viên này trực tiếp phụ trách
        /// </summary>
        /// <param name="id">ID của Huấn luyện viên</param>
        /// <returns>Danh sách hội viên đang học với Huấn luyện viên này</returns>
        /// <response code="200">Lấy danh sách học viên thành công</response>
        /// <response code="404">Không tìm thấy Huấn luyện viên</response>
        [HttpGet("{id:int}/members")]
        [ProducesResponseType(typeof(List<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserResponseDto>> GetAssignedMembers([FromRoute] int id)
        {
            var trainer = _trainerService.GetById(id);
            if (trainer == null)
            {
                return NotFound(new { message = $"Không tìm thấy Huấn luyện viên có Id = {id}" });
            }

            var members = _trainerService.GetAssignedMembers(id);
            return Ok(members);
        }

        /// <summary>
        /// Phân công một hội viên cho Huấn luyện viên hướng dẫn
        /// </summary>
        /// <param name="id">ID của Huấn luyện viên</param>
        /// <param name="memberId">ID của hội viên cần ghép cặp</param>
        /// <response code="200">Phân công thành công</response>
        /// <response code="400">HLV đã đủ số lượng học viên tối đa</response>
        /// <response code="404">Không tìm thấy HLV</response>
        [HttpPost("{id:int}/members/{memberId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AssignMember([FromRoute] int id, [FromRoute] int memberId)
        {
            var trainer = _trainerService.GetById(id);
            if (trainer == null)
            {
                return NotFound(new { message = $"Không tìm thấy Huấn luyện viên có Id = {id}" });
            }

            bool success = _trainerService.AssignMember(id, memberId);
            if (!success)
            {
                return BadRequest(new { message = $"Huấn luyện viên '{trainer.FullName}' đã đạt giới hạn tối đa ({trainer.MaxMembers} học viên)." });
            }

            return Ok(new { message = $"Đã phân công hội viên Id = {memberId} cho HLV {trainer.FullName} thành công." });
        }
    }
}
