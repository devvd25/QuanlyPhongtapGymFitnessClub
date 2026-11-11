using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs.Common;
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

        public async Task<PagedResult<User>> GetAllAsync(UserQueryParameters queryParams)
        {
            var query = _db.Users.Where(u => !u.IsDeleted).AsNoTracking().AsQueryable();

            // 1. SEARCH (Tìm kiếm tương đối)
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.Trim().ToLower();
                query = query.Where(u => u.Username.ToLower().Contains(term)
                                      || u.Email.ToLower().Contains(term)
                                      || u.FullName.ToLower().Contains(term));
            }

            // 2. FILTER (Lọc chính xác)
            if (!string.IsNullOrWhiteSpace(queryParams.Role))
            {
                // Entity User trong dự án Gym là Hội viên (Member)
                if (!queryParams.Role.Equals("Member", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(u => false);
                }
            }

            //if (queryParams.IsActive.HasValue)
            //{
            //    query = query.Where(u => u.IsActive == queryParams.IsActive.Value);
            //}

            // 3. SORTING
            query = queryParams.SortBy?.ToLower() switch
            {
                "email" => queryParams.IsDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),

                "createdat" => queryParams.IsDescending
                    ? query.OrderByDescending(u => u.CreatedAt)
                    : query.OrderBy(u => u.CreatedAt),

                _ => queryParams.IsDescending
                    ? query.OrderByDescending(u => u.Username)
                    : query.OrderBy(u => u.Username) // Mặc định sort theo UserName
            };

            // 4. COUNT (Tổng số dòng thỏa điều kiện search/filter)
            var totalCount = await query.CountAsync();

            // 5. PAGINATION (Chỉ lấy đúng PageSize dòng từ DB)
            var items = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResult<User>(items, totalCount, queryParams.PageNumber, queryParams.PageSize);
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
