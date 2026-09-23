using Buoi2_WebAPI.DTOs;

namespace Buoi2_WebAPI.Services
{
    /// <summary>
    /// Interface quản lý nghiệp vụ cho Nhân viên lễ tân phòng Gym (Buổi 2: Dependency Injection)
    /// </summary>
    public interface IStaffService
    {
        List<StaffResponseDto> GetAll(string? search, string? workShift, bool? isOnDuty);
        StaffResponseDto? GetById(int id);
        StaffResponseDto Create(StaffCreateDto dto);
        StaffResponseDto? Update(int id, StaffUpdateDto dto);
        bool Delete(int id);
        bool ToggleDutyStatus(int id);
    }
}
