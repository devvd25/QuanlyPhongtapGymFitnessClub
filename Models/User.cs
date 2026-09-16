using System.ComponentModel.DataAnnotations;      // Thư viện chứa các Attribute validation: [Required], [MaxLength], [EmailAddress]...
using System.ComponentModel.DataAnnotations.Schema; // Thư viện chứa [Key], [DatabaseGenerated] để cấu hình mapping với Database

namespace Buoi2_WebAPI.Models
{
    public class User
    {
        // ================= 1. KHÓA CHÍNH & TÀI KHOẢN ĐĂNG NHẬP =================

        [Key] // Đánh dấu đây là khóa chính (Primary Key)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database tự tăng Id (1, 2, 3...)
        public int Id { get; set; }

        [Required] // Bắt buộc nhập, không được null
        [MaxLength(50)] // Giới hạn tối đa 50 ký tự, tránh tạo cột nvarchar(MAX) trong SQL
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress] // Validate đúng định dạng email (phải có @)
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // Chuỗi ngẫu nhiên dùng để mã hóa mật khẩu

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); // Mật khẩu đã được mã hóa (không lưu plain text)

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Member"; // Phân quyền: Admin, Manager, Trainer, Member, Staff

        [MaxLength(500)]
        public string? RefreshToken { get; set; } // Token làm mới phiên đăng nhập
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // ================= 2. THÔNG TIN CÁ NHÂN =================

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone] // Validate định dạng số điện thoại
        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male"; // Male, Female, Other

        public DateTime? DateOfBirth { get; set; } // DateTime? = nullable, cho phép null

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? AvatarUrl { get; set; } // string? = nullable, không bắt buộc

        // ================= 3. THÔNG TIN THỂ TRẠNG & SỨC KHỎE GYM =================

        [Range(50, 250, ErrorMessage = "Chiều cao phải từ 50cm đến 250cm")] // Giới hạn giá trị số trong khoảng
        public double? HeightCm { get; set; }

        [Range(20, 300, ErrorMessage = "Cân nặng phải từ 20kg đến 300kg")]
        public double? WeightKg { get; set; }

        [MaxLength(500)]
        public string? HealthNotes { get; set; }

        [MaxLength(200)]
        public string? FitnessGoal { get; set; } // Mục tiêu tập: Giảm mỡ, Tăng cơ...

        // ================= 4. THÔNG TIN GÓI TẬP & THẺ HỘI VIÊN =================

        [MaxLength(50)]
        public string? MemberCardCode { get; set; } // Mã thẻ từ / QR Code để check-in

        [Required]
        [MaxLength(30)]
        public string MembershipPackage { get; set; } = "Basic"; // Basic, VIP, Diamond

        [Required]
        [MaxLength(20)]
        public string MembershipStatus { get; set; } = "Active"; // Active, Expired, Suspended

        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }

        [Range(0, 1000)] // Số buổi PT còn lại: từ 0 đến 1000
        public int RemainingPtSessions { get; set; } = 0;

        public int? AssignedTrainerId { get; set; } // FK đến Trainer phụ trách

        // ================= 5. QUẢN LÝ TRẠNG THÁI & AUDIT LOGS =================
        public bool IsActive { get; set; } = true; // true = hoạt động, false = khóa tài khoản
        public bool IsDeleted { get; set; } = false; // Xóa mềm (Soft Delete)

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
