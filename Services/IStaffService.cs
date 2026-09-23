using QuanlyPhongtapGymFitnessClub.DTOs;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    /// <summary>
    /// Interface quáº£n lĂ½ nghiá»‡p vá»¥ cho NhĂ¢n viĂªn lá»… tĂ¢n phĂ²ng Gym (Buá»•i 2: Dependency Injection)
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
