using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
