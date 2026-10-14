using QuanlyPhongtapGymFitnessClub.DTOs;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(UserLoginDto dto);
    }
}
