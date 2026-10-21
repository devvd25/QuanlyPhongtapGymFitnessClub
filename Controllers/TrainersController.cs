using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quan ly Huan luyen vien (Trainers)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TrainersController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/trainers
        [HttpGet]
        public ActionResult<List<TrainerResponseDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? specialty)
        {
            var query = _db.Trainers.Include(t => t.AssignedMembers).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(t => t.FullName.ToLower().Contains(s)
                                      || t.Username.ToLower().Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(specialty))
                query = query.Where(t => t.Specialty == specialty);

            var trainers = query.ToList().Select(t => new TrainerResponseDto
            {
                Id                  = t.Id,
                Username            = t.Username,
                FullName            = t.FullName,
                Email               = t.Email,
                Phone               = t.Phone,
                Gender              = t.Gender,
                DateOfBirth         = t.DateOfBirth,
                Specialty           = t.Specialty,
                Certifications      = t.Certifications,
                Rating              = t.Rating,
                HourlyRate          = t.HourlyRate,
                MaxMembers          = t.MaxMembers,
                CurrentMembersCount = t.AssignedMembers.Count,
                AssignedMemberIds   = t.AssignedMembers.Select(m => m.Id).ToList(),
                IsActive            = t.IsActive,
                CreatedAt           = t.CreatedAt
            }).ToList();

            return Ok(trainers);
        }

        // GET api/trainers/{id}
        [HttpGet("{id:int}")]
        public ActionResult<TrainerResponseDto> GetById(int id)
        {
            var t = _db.Trainers.Include(tr => tr.AssignedMembers)
                                .FirstOrDefault(tr => tr.Id == id);

            if (t == null)
                return NotFound(new { message = $"Khong tim thay Huan luyen vien co Id = {id}" });

            return Ok(new TrainerResponseDto
            {
                Id                  = t.Id,
                Username            = t.Username,
                FullName            = t.FullName,
                Email               = t.Email,
                Phone               = t.Phone,
                Gender              = t.Gender,
                DateOfBirth         = t.DateOfBirth,
                Specialty           = t.Specialty,
                Certifications      = t.Certifications,
                Rating              = t.Rating,
                HourlyRate          = t.HourlyRate,
                MaxMembers          = t.MaxMembers,
                CurrentMembersCount = t.AssignedMembers.Count,
                AssignedMemberIds   = t.AssignedMembers.Select(m => m.Id).ToList(),
                IsActive            = t.IsActive,
                CreatedAt           = t.CreatedAt
            });
        }

        // POST api/trainers
        [HttpPost]
        public ActionResult<TrainerResponseDto> Create([FromBody] TrainerCreateDto dto)
        {
            bool isExist = _db.Trainers.Any(t => t.Username == dto.Username || t.Email == dto.Email);
            if (isExist)
                return BadRequest(new { message = "Username hoac Email da duoc su dung." });

            var newTrainer = new Trainer
            {
                Username       = dto.Username,
                FullName       = dto.FullName,
                Email          = dto.Email,
                Phone          = dto.Phone,
                Gender         = dto.Gender,
                DateOfBirth    = dto.DateOfBirth,
                Specialty      = dto.Specialty,
                Certifications = dto.Certifications,
                HourlyRate     = dto.HourlyRate,
                MaxMembers     = dto.MaxMembers,
                IsActive       = true,
                CreatedAt      = DateTime.UtcNow
            };

            _db.Trainers.Add(newTrainer);
            _db.SaveChanges();

            var response = new TrainerResponseDto
            {
                Id                  = newTrainer.Id,
                Username            = newTrainer.Username,
                FullName            = newTrainer.FullName,
                Email               = newTrainer.Email,
                Phone               = newTrainer.Phone,
                Gender              = newTrainer.Gender,
                DateOfBirth         = newTrainer.DateOfBirth,
                Specialty           = newTrainer.Specialty,
                Certifications      = newTrainer.Certifications,
                Rating              = newTrainer.Rating,
                HourlyRate          = newTrainer.HourlyRate,
                MaxMembers          = newTrainer.MaxMembers,
                CurrentMembersCount = 0,
                AssignedMemberIds   = new List<int>(),
                IsActive            = newTrainer.IsActive,
                CreatedAt           = newTrainer.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT api/trainers/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] TrainerUpdateDto dto)
        {
            var trainer = _db.Trainers.FirstOrDefault(t => t.Id == id);

            if (trainer == null)
                return NotFound(new { message = $"Khong tim thay Huan luyen vien co Id = {id}" });

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != trainer.Email)
            {
                bool emailExists = _db.Trainers.Any(t => t.Email == dto.Email && t.Id != id);
                if (emailExists)
                    return BadRequest(new { message = "Email da duoc su dung boi Huan luyen vien khac." });
                trainer.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.FullName))       trainer.FullName       = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Phone))          trainer.Phone          = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.Specialty))      trainer.Specialty      = dto.Specialty;
            if (!string.IsNullOrWhiteSpace(dto.Certifications)) trainer.Certifications = dto.Certifications;
            if (dto.HourlyRate.HasValue)                        trainer.HourlyRate     = dto.HourlyRate.Value;
            if (dto.MaxMembers.HasValue)                        trainer.MaxMembers     = dto.MaxMembers.Value;
            if (dto.IsActive.HasValue)                          trainer.IsActive       = dto.IsActive.Value;
            trainer.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();

            return Ok(new { message = $"Cap nhat thanh cong Huan luyen vien Id = {id}" });
        }

        // DELETE api/trainers/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var trainer = _db.Trainers.FirstOrDefault(t => t.Id == id);

            if (trainer == null)
                return NotFound(new { message = $"Khong tim thay Huan luyen vien co Id = {id}" });

            _db.Trainers.Remove(trainer);
            _db.SaveChanges();

            return Ok(new { message = $"Da xoa Huan luyen vien Id = {id}" });
        }

        // GET api/trainers/{id}/members
        [HttpGet("{id:int}/members")]
        public ActionResult<List<UserResponseDto>> GetAssignedMembers(int id)
        {
            var trainer = _db.Trainers.Include(t => t.AssignedMembers)
                                      .FirstOrDefault(t => t.Id == id);

            if (trainer == null)
                return NotFound(new { message = $"Khong tim thay Huan luyen vien co Id = {id}" });

            var members = trainer.AssignedMembers.Select(u => new UserResponseDto
            {
                Id                = u.Id,
                Username          = u.Username,
                FullName          = u.FullName,
                Email             = u.Email,
                Phone             = u.Phone,
                MembershipPackage = u.MembershipPackage,
                MembershipStatus  = u.MembershipStatus,
                MembershipEndDate = u.MembershipEndDate,
                IsActive          = u.IsActive,
                CreatedAt         = u.CreatedAt
            }).ToList();

            return Ok(members);
        }

        // POST api/trainers/{id}/members/{memberId}
        [HttpPost("{id:int}/members/{memberId:int}")]
        public IActionResult AssignMember(int id, int memberId)
        {
            var trainer = _db.Trainers.Include(t => t.AssignedMembers)
                                      .FirstOrDefault(t => t.Id == id);

            if (trainer == null)
                return NotFound(new { message = $"Khong tim thay Huan luyen vien co Id = {id}" });

            if (trainer.AssignedMembers.Count >= trainer.MaxMembers)
                return BadRequest(new { message = $"HLV '{trainer.FullName}' da dat gioi han toi da ({trainer.MaxMembers} hoc vien)." });

            if (trainer.AssignedMembers.Any(m => m.Id == memberId))
                return Ok(new { message = "Hoi vien nay da duoc phan cong cho HLV nay roi." });

            var user = _db.Users.FirstOrDefault(u => u.Id == memberId);
            if (user == null)
                return NotFound(new { message = $"Khong tim thay Hoi vien co Id = {memberId}" });

            trainer.AssignedMembers.Add(user);
            trainer.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return Ok(new { message = $"Da phan cong Hoi vien Id = {memberId} cho HLV {trainer.FullName} thanh cong." });
        }
    }
}
