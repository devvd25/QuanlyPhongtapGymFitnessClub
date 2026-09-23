using Buoi2_WebAPI.DTOs;

namespace Buoi2_WebAPI.Services
{
    /// <summary>
    /// Interface quản lý nghiệp vụ cho Huấn luyện viên cá nhân (Buổi 2: Dependency Injection)
    /// </summary>
    public interface ITrainerService
    {
        List<TrainerResponseDto> GetAll(string? search, string? specialty, int? minExperienceYears);
        TrainerResponseDto? GetById(int id);
        TrainerResponseDto Create(TrainerCreateDto dto);
        TrainerResponseDto? Update(int id, TrainerUpdateDto dto);
        bool Delete(int id);
        List<UserResponseDto> GetAssignedMembers(int trainerId);
        bool AssignMember(int trainerId, int memberId);
    }
}
