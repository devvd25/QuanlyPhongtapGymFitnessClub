using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Repositories
{
    /// <summary>
    /// Lop Repository doc quyen thuc thi cac truy van Database cho bang Users
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllAsync(string? search = null, string? membershipStatus = null)
        {
            var query = _db.Users.Where(u => !u.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(s)
                                      || u.Username.ToLower().Contains(s)
                                      || u.Email.ToLower().Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(membershipStatus))
            {
                query = query.Where(u => u.MembershipStatus == membershipStatus);
            }

            return await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string username, string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => (u.Username == username || u.Email == email) && !u.IsDeleted);
        }

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _db.Users.Update(user);
        }

        public void Delete(User user)
        {
            _db.Users.Remove(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
