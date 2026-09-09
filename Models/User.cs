namespace Buoi2_WebAPI.Models
{
    public class User
    {
        // ================= 1. KHÓA CHÍNH & TÀI KHOẢN ĐĂNG NHẬP =================
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        // Phân quyền: Admin (Quản trị), Manager (Quản lý), Trainer (Huấn luyện viên/PT), Member (Hội viên), Staff (Lễ tân)
        public string Role { get; set; } = "Member";

        // Quản lý phiên đăng nhập & Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // ================= 2. THÔNG TIN CÁ NHÂN =================
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = "Male"; 
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } 

        // ================= 3. THÔNG TIN THỂ TRẠNG & SỨC KHỎE GYM =================
        public double? HeightCm { get; set; } 
        public double? WeightKg { get; set; } 
        public string? HealthNotes { get; set; } 
        public string? FitnessGoal { get; set; } // Mục tiêu tập luyện: Giảm mỡ, Tăng cơ, Tăng thể lực...

        // ================= 4. THÔNG TIN GÓI TẬP & THẺ HỘI VIÊN (MEMBERSHIP) =================
        // Mã thẻ từ / Mã vạch / QR Code để quét Check-in cửa ra vào phòng Gym
        public string? MemberCardCode { get; set; }

        // Gói tập: Basic, Standard, VIP, Premium, Diamond, Gói 1 Tháng, 6 Tháng, 1 Năm...
        public string MembershipPackage { get; set; } = "Basic";

        // Trạng thái gói: Active (Đang hoạt động), Expired (Hết hạn), Suspended (Tạm bảo lưu), Cancelled (Đã hủy)
        public string MembershipStatus { get; set; } = "Active";

        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public int RemainingPtSessions { get; set; } = 0; // Số buổi tập cùng Huấn luyện viên (PT) còn lại
        public int? AssignedTrainerId { get; set; } // ID của Huấn luyện viên phụ trách nếu có

        // ================= 5. QUẢN LÝ TRẠNG THÁI & AUDIT LOGS (NGÀY TẠO / UPDATE) =================
        public bool IsActive { get; set; } = true; // Kích hoạt / Khóa tài khoản
        public bool IsDeleted { get; set; } = false; // Xóa mềm (Soft Delete)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo tài khoản
        public DateTime? UpdatedAt { get; set; } // Ngày cập nhật thông tin gần nhất
        public DateTime? LastLoginAt { get; set; } // Thời điểm đăng nhập lần cuối
    }
}
