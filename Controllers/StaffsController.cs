using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quan ly Nhan vien le tan va Ban hang (Staffs)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StaffsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StaffsController(AppDbContext db)
        {
            _db = db;
        }

        // ------------------------------------------------------------------------
        // READ ALL (GET) - api/staffs
        // ------------------------------------------------------------------------
        [HttpGet]
        public ActionResult<List<StaffResponseDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? workShift,
            [FromQuery] bool? isOnDuty)
        {
            var query = _db.Staffs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(st => st.FullName.ToLower().Contains(s)
                                       || st.Username.ToLower().Contains(s)
                                       || st.Department.ToLower().Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(workShift))
                query = query.Where(st => st.WorkShift == workShift);

            if (isOnDuty.HasValue)
                query = query.Where(st => st.IsOnDuty == isOnDuty.Value);

            var staffs = query.Select(st => new StaffResponseDto
            {
                Id            = st.Id,
                Username      = st.Username,
                FullName      = st.FullName,
                Email         = st.Email,
                Phone         = st.Phone,
                Gender        = st.Gender,
                DateOfBirth   = st.DateOfBirth,
                WorkShift     = st.WorkShift,
                CounterNumber = st.CounterNumber,
                Department    = st.Department,
                HireDate      = st.HireDate,
                IsOnDuty      = st.IsOnDuty,
                IsActive      = st.IsActive,
                CreatedAt     = st.CreatedAt
            }).ToList();

            return Ok(staffs);
        }

        // ------------------------------------------------------------------------
        // READ BY ID (GET) - api/staffs/{id}
        // ------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public ActionResult<StaffResponseDto> GetById(int id)
        {
            var st = _db.Staffs.FirstOrDefault(s => s.Id == id);

            if (st == null)
                return NotFound(new { message = $"Khong tim thay nhan vien co Id = {id}" });

            return Ok(new StaffResponseDto
            {
                Id            = st.Id,
                Username      = st.Username,
                FullName      = st.FullName,
                Email         = st.Email,
                Phone         = st.Phone,
                Gender        = st.Gender,
                DateOfBirth   = st.DateOfBirth,
                WorkShift     = st.WorkShift,
                CounterNumber = st.CounterNumber,
                Department    = st.Department,
                HireDate      = st.HireDate,
                IsOnDuty      = st.IsOnDuty,
                IsActive      = st.IsActive,
                CreatedAt     = st.CreatedAt
            });
        }

        // ------------------------------------------------------------------------
        // CREATE (POST) - api/staffs
        // ------------------------------------------------------------------------
        [HttpPost]
        public ActionResult<StaffResponseDto> Create([FromBody] StaffCreateDto dto)
        {
            bool isExist = _db.Staffs.Any(s => s.Username == dto.Username || s.Email == dto.Email);
            if (isExist)
                return BadRequest(new { message = "Username hoac Email da duoc su dung." });

            var newStaff = new Staff
            {
                Username      = dto.Username,
                FullName      = dto.FullName,
                Email         = dto.Email,
                Phone         = dto.Phone,
                Gender        = dto.Gender,
                DateOfBirth   = dto.DateOfBirth,
                WorkShift     = dto.WorkShift,
                CounterNumber = dto.CounterNumber,
                Department    = dto.Department,
                HireDate      = DateTime.UtcNow,
                IsOnDuty      = false,
                IsActive      = true,
                CreatedAt     = DateTime.UtcNow
            };

            _db.Staffs.Add(newStaff);
            _db.SaveChanges();

            var response = new StaffResponseDto
            {
                Id            = newStaff.Id,
                Username      = newStaff.Username,
                FullName      = newStaff.FullName,
                Email         = newStaff.Email,
                Phone         = newStaff.Phone,
                Gender        = newStaff.Gender,
                DateOfBirth   = newStaff.DateOfBirth,
                WorkShift     = newStaff.WorkShift,
                CounterNumber = newStaff.CounterNumber,
                Department    = newStaff.Department,
                HireDate      = newStaff.HireDate,
                IsOnDuty      = newStaff.IsOnDuty,
                IsActive      = newStaff.IsActive,
                CreatedAt     = newStaff.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = newStaff.Id }, response);
        }

        // ------------------------------------------------------------------------
        // UPDATE (PUT) - api/staffs/{id}
        // ------------------------------------------------------------------------
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] StaffUpdateDto dto)
        {
            var staff = _db.Staffs.FirstOrDefault(s => s.Id == id);

            if (staff == null)
                return NotFound(new { message = $"Khong tim thay nhan vien co Id = {id}" });

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != staff.Email)
            {
                bool emailExists = _db.Staffs.Any(s => s.Email == dto.Email && s.Id != id);
                if (emailExists)
                    return BadRequest(new { message = "Email da duoc su dung boi nhan vien khac." });
                staff.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.FullName))   staff.FullName   = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Phone))      staff.Phone      = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.WorkShift))  staff.WorkShift  = dto.WorkShift;
            if (!string.IsNullOrWhiteSpace(dto.Department)) staff.Department = dto.Department;
            if (dto.CounterNumber.HasValue)                 staff.CounterNumber = dto.CounterNumber.Value;
            if (dto.IsOnDuty.HasValue)                      staff.IsOnDuty      = dto.IsOnDuty.Value;
            if (dto.IsActive.HasValue)                      staff.IsActive      = dto.IsActive.Value;
            staff.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();

            return Ok(new { message = $"Cap nhat thanh cong nhan vien Id = {id}" });
        }

        // ------------------------------------------------------------------------
        // DELETE - api/staffs/{id}
        // ------------------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var staff = _db.Staffs.FirstOrDefault(s => s.Id == id);

            if (staff == null)
                return NotFound(new { message = $"Khong tim thay nhan vien co Id = {id}" });

            bool hasInvoices = _db.Invoices.Any(i => i.StaffId == id);
            if (hasInvoices)
                return BadRequest(new { message = "Khong the xoa nhan vien nay vi da co hoa don duoc lap boi nhan vien. Ban co the cap nhat IsActive = false de khoa." });

            _db.Staffs.Remove(staff);
            _db.SaveChanges();

            return Ok(new { message = $"Da xoa nhan vien Id = {id}" });
        }

        // ------------------------------------------------------------------------
        // TOGGLE DUTY STATUS (PATCH) - api/staffs/{id}/toggle-duty
        // ------------------------------------------------------------------------
        [HttpPatch("{id:int}/toggle-duty")]
        public IActionResult ToggleDutyStatus(int id)
        {
            var staff = _db.Staffs.FirstOrDefault(s => s.Id == id);

            if (staff == null)
                return NotFound(new { message = $"Khong tim thay nhan vien co Id = {id}" });

            staff.IsOnDuty  = !staff.IsOnDuty;
            staff.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return Ok(new
            {
                message  = $"Da cap nhat trang thai truc ca cua {staff.FullName}.",
                isOnDuty = staff.IsOnDuty
            });
        }
    }
}
