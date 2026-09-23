using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;
using Microsoft.AspNetCore.Mvc;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quáº£n lĂ½ NhĂ¢n viĂªn lá»… tĂ¢n vĂ  BĂ¡n hĂ ng (Front Desk Staff)
    /// Ăp dá»¥ng Ä‘áº§y Ä‘á»§: Dependency Injection (Buá»•i 2), RESTful CRUD (Buá»•i 3), DTOs vĂ  Validation (Buá»•i 4)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        /// <summary>
        /// Khá»Ÿi táº¡o StaffsController vá»›i Dependency Injection cá»§a IStaffService
        /// </summary>
        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        /// <summary>
        /// Láº¥y danh sĂ¡ch táº¥t cáº£ nhĂ¢n viĂªn phĂ²ng gym (há»— trá»£ lá»c theo ca lĂ m viá»‡c vĂ  tráº¡ng thĂ¡i trá»±c)
        /// </summary>
        /// <param name="search">TĂ¬m kiáº¿m theo há» tĂªn, username hoáº·c phĂ²ng ban</param>
        /// <param name="workShift">Lá»c theo ca trá»±c: Morning, Afternoon, Evening, FullDay</param>
        /// <param name="isOnDuty">Lá»c theo tráº¡ng thĂ¡i Ä‘ang trá»±c quáº§y (true: Ä‘ang trá»±c, false: nghá»‰ ca)</param>
        /// <returns>Danh sĂ¡ch nhĂ¢n viĂªn</returns>
        /// <response code="200">Láº¥y danh sĂ¡ch thĂ nh cĂ´ng</response>
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
        /// Láº¥y thĂ´ng tin nhĂ¢n viĂªn theo ID
        /// </summary>
        /// <param name="id">ID cá»§a nhĂ¢n viĂªn</param>
        /// <returns>ThĂ´ng tin nhĂ¢n viĂªn</returns>
        /// <response code="200">TĂ¬m tháº¥y nhĂ¢n viĂªn</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn vá»›i ID chá»‰ Ä‘á»‹nh</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StaffResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StaffResponseDto> GetById([FromRoute] int id)
        {
            var staff = _staffService.GetById(id);
            if (staff == null)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn cĂ³ Id = {id}" });
            }
            return Ok(staff);
        }

        /// <summary>
        /// ThĂªm má»›i má»™t nhĂ¢n viĂªn lá»… tĂ¢n vĂ o há»‡ thá»‘ng
        /// </summary>
        /// <remarks>
        /// Dá»¯ liá»‡u Ä‘áº§u vĂ o báº¯t buá»™c:
        /// - Äá»™ tuá»•i há»£p lá»‡ tá»« 18 Ä‘áº¿n 65 (Custom Validation [GymAgeValidation])
        /// - Ca lĂ m viá»‡c: Morning, Afternoon, Evening, FullDay
        /// - Email, SÄT Ä‘Ăºng Ä‘á»‹nh dáº¡ng chuáº©n
        /// </remarks>
        /// <param name="dto">ThĂ´ng tin nhĂ¢n viĂªn cáº§n táº¡o</param>
        /// <returns>NhĂ¢n viĂªn vá»«a táº¡o kĂ¨m Location header</returns>
        /// <response code="201">Táº¡o má»›i thĂ nh cĂ´ng</response>
        /// <response code="400">Dá»¯ liá»‡u vi pháº¡m quy táº¯c validation</response>
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
        /// Cáº­p nháº­t thĂ´ng tin nhĂ¢n viĂªn theo ID
        /// </summary>
        /// <param name="id">ID nhĂ¢n viĂªn cáº§n cáº­p nháº­t</param>
        /// <param name="dto">Dá»¯ liá»‡u cáº­p nháº­t</param>
        /// <returns>KhĂ´ng cĂ³ ná»™i dung tráº£ vá» náº¿u cáº­p nháº­t thĂ nh cĂ´ng (204 NoContent)</returns>
        /// <response code="204">Cáº­p nháº­t thĂ nh cĂ´ng</response>
        /// <response code="400">Dá»¯ liá»‡u cáº­p nháº­t khĂ´ng há»£p lá»‡</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn Ä‘á»ƒ cáº­p nháº­t</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromRoute] int id, [FromBody] StaffUpdateDto dto)
        {
            var updated = _staffService.Update(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn cĂ³ Id = {id} Ä‘á»ƒ cáº­p nháº­t" });
            }

            return NoContent();
        }

        /// <summary>
        /// XĂ³a má»™t nhĂ¢n viĂªn khá»i há»‡ thá»‘ng theo ID
        /// </summary>
        /// <param name="id">ID nhĂ¢n viĂªn cáº§n xĂ³a</param>
        /// <returns>KhĂ´ng cĂ³ ná»™i dung tráº£ vá» náº¿u xĂ³a thĂ nh cĂ´ng (204 NoContent)</returns>
        /// <response code="204">XĂ³a thĂ nh cĂ´ng</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn Ä‘á»ƒ xĂ³a</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            bool isDeleted = _staffService.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn cĂ³ Id = {id} Ä‘á»ƒ xĂ³a" });
            }

            return NoContent();
        }

        /// <summary>
        /// Äá»•i tráº¡ng thĂ¡i trá»±c ca cá»§a nhĂ¢n viĂªn (Äang trá»±c quáº§y / Nghá»‰ ca)
        /// </summary>
        /// <param name="id">ID cá»§a nhĂ¢n viĂªn</param>
        /// <response code="200">Äá»•i tráº¡ng thĂ¡i trá»±c ca thĂ nh cĂ´ng</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn</response>
        [HttpPatch("{id:int}/toggle-duty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ToggleDutyStatus([FromRoute] int id)
        {
            bool success = _staffService.ToggleDutyStatus(id);
            if (!success)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y nhĂ¢n viĂªn cĂ³ Id = {id}" });
            }

            var staff = _staffService.GetById(id);
            return Ok(new
            {
                message = $"ÄĂ£ cáº­p nháº­t tráº¡ng thĂ¡i trá»±c ca cá»§a nhĂ¢n viĂªn {staff?.FullName} thĂ nh cĂ´ng.",
                isOnDuty = staff?.IsOnDuty
            });
        }
    }
}
