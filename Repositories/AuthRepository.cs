using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }
    }
}
