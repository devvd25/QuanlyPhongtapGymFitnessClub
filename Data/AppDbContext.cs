using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Data
{
    // AppDbContext: Đại diện cho phiên làm việc với Database (Database Session)
    // Kế thừa từ DbContext của Entity Framework Core
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ===== KHAI BÁO CÁC BẢNG =====
        public DbSet<User> Users { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Unique Index: tránh trùng Username / Email ──────────────────
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ── Unique Index: InvoiceCode không được trùng ──────────────────
            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceCode)
                .IsUnique();

            // ── Decimal precision: tránh cảnh báo truncation ────────────────
            modelBuilder.Entity<Trainer>()
                .Property(t => t.HourlyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.SubTotal)
                .HasPrecision(18, 2);

            // ── Quan hệ User (hội viên) ↔ Trainer (N-1) ────────────────────
            // Nhiều User có thể thuộc về 1 Trainer
            // Khi xóa Trainer → AssignedTrainerId của User = NULL (không mất data)
            modelBuilder.Entity<User>()
                .HasOne(u => u.AssignedTrainer)
                .WithMany(t => t.AssignedMembers)
                .HasForeignKey(u => u.AssignedTrainerId)
                .OnDelete(DeleteBehavior.SetNull);

            // ── Quan hệ Invoice ↔ User (N-1) ────────────────────────────────
            // Nhiều Invoice có thể thuộc về 1 User (nullable: khách vãng lai)
            // Khi xóa User → Invoice.UserId = NULL
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ── Quan hệ Invoice ↔ Staff (N-1) ────────────────────────────────
            // Nhiều Invoice do 1 Staff tạo
            // Khi xóa Staff → không cho xóa nếu còn hóa đơn (Restrict)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Staff)
                .WithMany()
                .HasForeignKey(i => i.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Quan hệ InvoiceItem ↔ Invoice (N-1) ─────────────────────────
            // Xóa Invoice → xóa luôn tất cả InvoiceItems (Cascade)
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Quan hệ InvoiceItem ↔ Product (N-1) ─────────────────────────
            // Khi xóa Product → không cho xóa nếu còn trong hóa đơn (Restrict)
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Product)
                .WithMany()
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
