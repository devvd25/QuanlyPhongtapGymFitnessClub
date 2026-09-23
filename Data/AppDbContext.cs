using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Data
{
    // AppDbContext: Đại diện cho phiên làm việc với Database (Database Session)
    // Kế thừa từ DbContext của Entity Framework Core
    public class AppDbContext : DbContext
    {
        // Constructor nhận DbContextOptions (chứa ConnectionString, Provider...) được truyền từ Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Khai báo bảng Users trong SQL Server tương ứng với Model User
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình ràng buộc duy nhất (Unique Index) để không bị trùng Username hoặc Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}