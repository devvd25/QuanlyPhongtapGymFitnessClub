using Buoi2_WebAPI.DTOs;
using Buoi2_WebAPI.Models;

namespace Buoi2_WebAPI.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ cho Nhân viên lễ tân phòng Gym (Đăng ký Scoped trong Program.cs)
    /// </summary>
    public class StaffService : IStaffService
    {
        private static readonly List<Staff> _staffs = new()
        {
            new Staff
            {
                Id = 1,
                Username = "staff_linh",
                FullName = "Trần Khánh Linh",
                Email = "linh.tran@fitnessclub.vn",
                Phone = "0918889900",
                Gender = "Female",
                DateOfBirth = new DateTime(2001, 5, 14),
                WorkShift = "Morning",
                CounterNumber = 1,
                Department = "Reception",
                HireDate = new DateTime(2025, 2, 1),
                IsOnDuty = true,
                IsActive = true,
                CreatedAt = new DateTime(2025, 2, 1)
            },
            new Staff
            {
                Id = 2,
                Username = "staff_minh",
                FullName = "Ngô Nhật Minh",
                Email = "minh.ngo@fitnessclub.vn",
                Phone = "0947778899",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 9, 30),
                WorkShift = "Afternoon",
                CounterNumber = 2,
                Department = "Sales",
                HireDate = new DateTime(2024, 10, 15),
                IsOnDuty = false,
                IsActive = true,
                CreatedAt = new DateTime(2024, 10, 15)
            },
            new Staff
            {
                Id = 3,
                Username = "staff_thao",
                FullName = "Phạm Phương Thảo",
                Email = "thao.pham@fitnessclub.vn",
                Phone = "0962223344",
                Gender = "Female",
                DateOfBirth = new DateTime(2002, 12, 5),
                WorkShift = "Evening",
                CounterNumber = 1,
                Department = "Reception",
                HireDate = new DateTime(2025, 4, 10),
                IsOnDuty = true,
                IsActive = true,
                CreatedAt = new DateTime(2025, 4, 10)
            }
        };

        public List<StaffResponseDto> GetAll(string? search, string? workShift, bool? isOnDuty)
        {
            var query = _staffs.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)
                                      || s.Username.Contains(search, StringComparison.OrdinalIgnoreCase)
                                      || s.Department.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(workShift))
            {
                query = query.Where(s => s.WorkShift.Equals(workShift, StringComparison.OrdinalIgnoreCase));
            }

            if (isOnDuty.HasValue)
            {
                query = query.Where(s => s.IsOnDuty == isOnDuty.Value);
            }

            return query.Select(MapToResponseDto).ToList();
        }

        public StaffResponseDto? GetById(int id)
        {
            var staff = _staffs.FirstOrDefault(s => s.Id == id);
            return staff == null ? null : MapToResponseDto(staff);
        }

        public StaffResponseDto Create(StaffCreateDto dto)
        {
            var newStaff = new Staff
            {
                Id = _staffs.Any() ? _staffs.Max(s => s.Id) + 1 : 1,
                Username = dto.Username,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                WorkShift = dto.WorkShift,
                CounterNumber = dto.CounterNumber,
                Department = dto.Department,
                HireDate = DateTime.UtcNow,
                IsOnDuty = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _staffs.Add(newStaff);
            return MapToResponseDto(newStaff);
        }

        public StaffResponseDto? Update(int id, StaffUpdateDto dto)
        {
            var staff = _staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.FullName)) staff.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) staff.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) staff.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.WorkShift)) staff.WorkShift = dto.WorkShift;
            if (dto.CounterNumber.HasValue) staff.CounterNumber = dto.CounterNumber.Value;
            if (!string.IsNullOrWhiteSpace(dto.Department)) staff.Department = dto.Department;
            if (dto.IsOnDuty.HasValue) staff.IsOnDuty = dto.IsOnDuty.Value;
            if (dto.IsActive.HasValue) staff.IsActive = dto.IsActive.Value;
            staff.UpdatedAt = DateTime.UtcNow;

            return MapToResponseDto(staff);
        }

        public bool Delete(int id)
        {
            var staff = _staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return false;

            _staffs.Remove(staff);
            return true;
        }

        public bool ToggleDutyStatus(int id)
        {
            var staff = _staffs.FirstOrDefault(s => s.Id == id);
            if (staff == null) return false;

            staff.IsOnDuty = !staff.IsOnDuty;
            staff.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        private static StaffResponseDto MapToResponseDto(Staff s) => new()
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
        };
    }
}
