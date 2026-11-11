using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.DTOs.Common;

namespace QuanlyPhongtapGymFitnessClub.Services
{
    /// <summary>
    /// Interface Service quan ly nghiep vu cho Hoi vien (Users)
    /// </summary>
    public interface IUserService
    {
        Task<PagedResult<UserResponseDto>> GetAllAsync(UserQueryParameters queryParams);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<(bool IsSuccess, string Message, UserResponseDto? Data)> RegisterAsync(UserRegisterDto request);
        Task<(bool IsSuccess, string Message, string? Token, string? Username, string? Role)> LoginAsync(UserLoginDto request);
        Task<(bool IsSuccess, string Message)> UpdateAsync(int id, UserUpdateDto request);
        Task<(bool IsSuccess, string Message)> DeleteAsync(int id);
    }
}
