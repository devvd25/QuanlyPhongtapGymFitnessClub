using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Repositories
{
    /// <summary>
    /// Interface Repository danh cho User (Hoi vien)
    /// Dinh nghia cac thao tac truc tiep voi Database
    /// </summary>
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(string? search = null, string? membershipStatus = null);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameOrEmailAsync(string username, string email);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<bool> SaveChangesAsync();
    }
}
