using QuanlyPhongtapGymFitnessClub.DTOs;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    /// <summary>
    /// Interface Service quan ly nghiep vu cho Hoi vien (Users)
    /// </summary>
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllAsync(string? search = null, string? membershipStatus = null);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<(bool IsSuccess, string Message, UserResponseDto? Data)> RegisterAsync(UserRegisterDto request);
        Task<(bool IsSuccess, string Message, string? Token, string? Username, string? Role)> LoginAsync(UserLoginDto request);
        Task<(bool IsSuccess, string Message)> UpdateAsync(int id, UserUpdateDto request);
        Task<(bool IsSuccess, string Message)> DeleteAsync(int id);
    }
}
