using QuanlyPhongtapGymFitnessClub.DTOs.Common;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Repositories
{
    /// <summary>
    /// Interface Repository danh cho User (Hoi vien)
    /// Dinh nghia cac thao tac truc tiep voi Database
    /// </summary>
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetAllAsync(UserQueryParameters queryParams);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameOrEmailAsync(string username, string email);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<bool> SaveChangesAsync();
    }
}
