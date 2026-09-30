using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    public class StaffService : IStaffService
    {
        private readonly AppDbContext _context;

        public StaffService(AppDbContext context)
        {
            _context = context;
        }

        public List<StaffResponseDto> GetAll(string? search, string? workShift, bool? isOnDuty)
        {
            var query = _context.Staffs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(s => s.FullName.ToLower().Contains(searchLower) || s.Username.ToLower().Contains(searchLower) || s.Department.ToLower().Contains(searchLower));
            }

            if (!string.IsNullOrWhiteSpace(workShift))
            {
                query = query.Where(s => s.WorkShift == workShift);
            }

            if (isOnDuty.HasValue)
            {
                query = query.Where(s => s.IsOnDuty == isOnDuty.Value);
            }

            return query.Select(s => new StaffResponseDto
            {
                Id = s.Id,
                Username = s.Username,
                FullName = s.FullName,
                Email = s.Email,
                Phone = s.Phone,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                WorkShift = s.WorkShift,
                CounterNumber = s.CounterNumber,
                Department = s.Department,
                HireDate = s.HireDate,
                IsOnDuty = s.IsOnDuty,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            }).ToList();
        }

        public StaffResponseDto? GetById(int id)
        {
            var staff = _context.Staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return null;

            return new StaffResponseDto
            {
                Id = staff.Id,
                Username = staff.Username,
                FullName = staff.FullName,
                Email = staff.Email,
                Phone = staff.Phone,
                Gender = staff.Gender,
                DateOfBirth = staff.DateOfBirth,
                WorkShift = staff.WorkShift,
                CounterNumber = staff.CounterNumber,
                Department = staff.Department,
                HireDate = staff.HireDate,
                IsOnDuty = staff.IsOnDuty,
                IsActive = staff.IsActive,
                CreatedAt = staff.CreatedAt
            };
        }

        public StaffResponseDto Create(StaffCreateDto dto)
        {
            var newStaff = new Staff
            {
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                WorkShift = dto.WorkShift,
                CounterNumber = dto.CounterNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsOnDuty = false
            };

            _context.Staffs.Add(newStaff);
            _context.SaveChanges();

            return GetById(newStaff.Id)!;
        }

        public StaffResponseDto? Update(int id, StaffUpdateDto dto)
        {
            var staff = _context.Staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.FullName)) staff.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) staff.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) staff.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.WorkShift)) staff.WorkShift = dto.WorkShift;
            if (dto.CounterNumber.HasValue) staff.CounterNumber = dto.CounterNumber.Value;

            staff.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return GetById(id);
        }

        public bool Delete(int id)
        {
            var staff = _context.Staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return false;

            _context.Staffs.Remove(staff);
            _context.SaveChanges();
            return true;
        }

        public bool ToggleDutyStatus(int id)
        {
            var staff = _context.Staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return false;

            staff.IsOnDuty = !staff.IsOnDuty;
            staff.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return true;
        }
    }
}
