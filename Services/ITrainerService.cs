using QuanlyPhongtapGymFitnessClub.DTOs;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    /// <summary>
    /// Interface quáº£n lĂ½ nghiá»‡p vá»¥ cho Huáº¥n luyá»‡n viĂªn cĂ¡ nhĂ¢n (Buá»•i 2: Dependency Injection)
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
