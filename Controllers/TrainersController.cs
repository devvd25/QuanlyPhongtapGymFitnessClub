using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Services;
using Microsoft.AspNetCore.Mvc;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quáº£n lĂ½ Huáº¥n luyá»‡n viĂªn cĂ¡ nhĂ¢n (Personal Trainers)
    /// Ăp dá»¥ng Ä‘áº§y Ä‘á»§: Dependency Injection (Buá»•i 2), RESTful CRUD vĂ  Nested Route (Buá»•i 3), DTOs vĂ  Validation (Buá»•i 4)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        /// <summary>
        /// TiĂªm ITrainerService vĂ o Controller thĂ´ng qua Constructor (Buá»•i 2: Dependency Injection)
        /// </summary>
        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        /// <summary>
        /// Láº¥y danh sĂ¡ch táº¥t cáº£ Huáº¥n luyá»‡n viĂªn (há»— trá»£ tĂ¬m kiáº¿m vĂ  lá»c dá»¯ liá»‡u)
        /// </summary>
        /// <param name="search">Tá»« khĂ³a tĂ¬m kiáº¿m theo tĂªn, username hoáº·c chuyĂªn mĂ´n</param>
        /// <param name="specialty">Lá»c theo chuyĂªn mĂ´n (Gym, Fitness, Bodybuilding, Yoga, Boxing, Pilates)</param>
        /// <param name="minExperienceYears">Lá»c theo sá»‘ nÄƒm kinh nghiá»‡m tá»‘i thiá»ƒu</param>
        /// <returns>Danh sĂ¡ch Huáº¥n luyá»‡n viĂªn phĂ¹ há»£p</returns>
        /// <response code="200">Láº¥y danh sĂ¡ch thĂ nh cĂ´ng</response>
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
        /// Láº¥y thĂ´ng tin chi tiáº¿t má»™t Huáº¥n luyá»‡n viĂªn theo ID
        /// </summary>
        /// <param name="id">ID cá»§a Huáº¥n luyá»‡n viĂªn (pháº£i lĂ  sá»‘ nguyĂªn)</param>
        /// <returns>ThĂ´ng tin Huáº¥n luyá»‡n viĂªn</returns>
        /// <response code="200">TĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn vá»›i ID chá»‰ Ä‘á»‹nh</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TrainerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TrainerResponseDto> GetById([FromRoute] int id)
        {
            var trainer = _trainerService.GetById(id);
            if (trainer == null)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn cĂ³ Id = {id}" });
            }
            return Ok(trainer);
        }

        /// <summary>
        /// ThĂªm má»›i má»™t Huáº¥n luyá»‡n viĂªn vĂ o phĂ²ng gym
        /// </summary>
        /// <remarks>
        /// Dá»¯ liá»‡u Ä‘áº§u vĂ o sáº½ Ä‘Æ°á»£c tá»± Ä‘á»™ng kiá»ƒm tra tĂ­nh há»£p lá»‡:
        /// - Äá»™ tuá»•i pháº£i tá»« 18 Ä‘áº¿n 60 (Custom Validation [GymAgeValidation])
        /// - Sá»‘ lÆ°á»£ng há»c viĂªn tá»‘i Ä‘a pháº£i lĂ  sá»‘ cháºµn (Custom Validation [MustBeEven])
        /// - Email, SÄT, Username theo Ä‘Ăºng Ä‘á»‹nh dáº¡ng chuáº©n
        /// </remarks>
        /// <param name="dto">ThĂ´ng tin Huáº¥n luyá»‡n viĂªn cáº§n táº¡o má»›i</param>
        /// <returns>Huáº¥n luyá»‡n viĂªn vá»«a Ä‘Æ°á»£c táº¡o kĂ¨m URL Location</returns>
        /// <response code="201">Táº¡o má»›i thĂ nh cĂ´ng, tráº£ vá» Location header vĂ  dá»¯ liá»‡u vá»«a táº¡o</response>
        /// <response code="400">Dá»¯ liá»‡u Ä‘áº§u vĂ o khĂ´ng há»£p lá»‡ hoáº·c vi pháº¡m quy táº¯c validation</response>
        [HttpPost]
        [ProducesResponseType(typeof(TrainerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TrainerResponseDto> Create([FromBody] TrainerCreateDto dto)
        {
            var created = _trainerService.Create(dto);

            // Tráº£ vá» 201 Created kĂ¨m Header Location trá» tá»›i endpoint GET /api/trainers/{id}
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Cáº­p nháº­t thĂ´ng tin Huáº¥n luyá»‡n viĂªn theo ID
        /// </summary>
        /// <param name="id">ID cá»§a Huáº¥n luyá»‡n viĂªn cáº§n cáº­p nháº­t</param>
        /// <param name="dto">Dá»¯ liá»‡u cáº­p nháº­t má»›i</param>
        /// <returns>KhĂ´ng cĂ³ ná»™i dung tráº£ vá» náº¿u cáº­p nháº­t thĂ nh cĂ´ng (204 NoContent)</returns>
        /// <response code="204">Cáº­p nháº­t thĂ nh cĂ´ng (No Content)</response>
        /// <response code="400">Dá»¯ liá»‡u cáº­p nháº­t khĂ´ng há»£p lá»‡</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn Ä‘á»ƒ cáº­p nháº­t</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromRoute] int id, [FromBody] TrainerUpdateDto dto)
        {
            var updated = _trainerService.Update(id, dto);
            if (updated == null)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn cĂ³ Id = {id} Ä‘á»ƒ cáº­p nháº­t" });
            }

            return NoContent(); // 204 NoContent chuáº©n RESTful
        }

        /// <summary>
        /// XĂ³a má»™t Huáº¥n luyá»‡n viĂªn khá»i há»‡ thá»‘ng theo ID
        /// </summary>
        /// <param name="id">ID cá»§a Huáº¥n luyá»‡n viĂªn cáº§n xĂ³a</param>
        /// <returns>KhĂ´ng cĂ³ ná»™i dung tráº£ vá» náº¿u xĂ³a thĂ nh cĂ´ng (204 NoContent)</returns>
        /// <response code="204">XĂ³a thĂ nh cĂ´ng (No Content)</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn Ä‘á»ƒ xĂ³a</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            bool isDeleted = _trainerService.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn cĂ³ Id = {id} Ä‘á»ƒ xĂ³a" });
            }

            return NoContent(); // 204 NoContent chuáº©n RESTful
        }

        /// <summary>
        /// Nested Route: Láº¥y danh sĂ¡ch há»™i viĂªn do Huáº¥n luyá»‡n viĂªn nĂ y trá»±c tiáº¿p phá»¥ trĂ¡ch
        /// </summary>
        /// <param name="id">ID cá»§a Huáº¥n luyá»‡n viĂªn</param>
        /// <returns>Danh sĂ¡ch há»™i viĂªn Ä‘ang há»c vá»›i Huáº¥n luyá»‡n viĂªn nĂ y</returns>
        /// <response code="200">Láº¥y danh sĂ¡ch há»c viĂªn thĂ nh cĂ´ng</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn</response>
        [HttpGet("{id:int}/members")]
        [ProducesResponseType(typeof(List<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserResponseDto>> GetAssignedMembers([FromRoute] int id)
        {
            var trainer = _trainerService.GetById(id);
            if (trainer == null)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn cĂ³ Id = {id}" });
            }

            var members = _trainerService.GetAssignedMembers(id);
            return Ok(members);
        }

        /// <summary>
        /// PhĂ¢n cĂ´ng má»™t há»™i viĂªn cho Huáº¥n luyá»‡n viĂªn hÆ°á»›ng dáº«n
        /// </summary>
        /// <param name="id">ID cá»§a Huáº¥n luyá»‡n viĂªn</param>
        /// <param name="memberId">ID cá»§a há»™i viĂªn cáº§n ghĂ©p cáº·p</param>
        /// <response code="200">PhĂ¢n cĂ´ng thĂ nh cĂ´ng</response>
        /// <response code="400">HLV Ä‘Ă£ Ä‘á»§ sá»‘ lÆ°á»£ng há»c viĂªn tá»‘i Ä‘a</response>
        /// <response code="404">KhĂ´ng tĂ¬m tháº¥y HLV</response>
        [HttpPost("{id:int}/members/{memberId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AssignMember([FromRoute] int id, [FromRoute] int memberId)
        {
            var trainer = _trainerService.GetById(id);
            if (trainer == null)
            {
                return NotFound(new { message = $"KhĂ´ng tĂ¬m tháº¥y Huáº¥n luyá»‡n viĂªn cĂ³ Id = {id}" });
            }

            bool success = _trainerService.AssignMember(id, memberId);
            if (!success)
            {
                return BadRequest(new { message = $"Huáº¥n luyá»‡n viĂªn '{trainer.FullName}' Ä‘Ă£ Ä‘áº¡t giá»›i háº¡n tá»‘i Ä‘a ({trainer.MaxMembers} há»c viĂªn)." });
            }

            return Ok(new { message = $"ÄĂ£ phĂ¢n cĂ´ng há»™i viĂªn Id = {memberId} cho HLV {trainer.FullName} thĂ nh cĂ´ng." });
        }
    }
}
