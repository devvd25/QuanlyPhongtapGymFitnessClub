using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;
using Microsoft.AspNetCore.Mvc;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quản lý Huấn luyện viên cá nhân
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        /// <summary>
        /// Lấy danh sách Huấn luyện viên
        /// </summary>
        /// <param name="search">Tìm theo tên, username hoặc chuyên môn</param>
        /// <param name="specialty">Lọc theo chuyên môn</param>
        /// <param name="minExperienceYears">Lọc theo số năm kinh nghiệm</param>
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
        /// Lấy thông tin Huấn luyện viên theo ID
        /// </summary>
        /// <param name="id">ID Huấn luyện viên</param>
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
        /// Thêm Huấn luyện viên mới
        /// </summary>
        /// <param name="dto">Thông tin Huấn luyện viên</param>
        [HttpPost]
        [ProducesResponseType(typeof(TrainerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TrainerResponseDto> Create([FromBody] TrainerCreateDto dto)
        {
            var created = _trainerService.Create(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Cập nhật thông tin Huấn luyện viên
        /// </summary>
        /// <param name="id">ID Huấn luyện viên</param>
        /// <param name="dto">Dữ liệu cập nhật</param>
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

            return NoContent();
        }

        /// <summary>
        /// Xóa Huấn luyện viên theo ID
        /// </summary>
        /// <param name="id">ID Huấn luyện viên</param>
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

            return NoContent();
        }

        /// <summary>
        /// Lấy danh sách học viên của Huấn luyện viên
        /// </summary>
        /// <param name="id">ID Huấn luyện viên</param>
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
        /// Phân công học viên cho Huấn luyện viên
        /// </summary>
        /// <param name="id">ID Huấn luyện viên</param>
        /// <param name="memberId">ID học viên</param>
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
