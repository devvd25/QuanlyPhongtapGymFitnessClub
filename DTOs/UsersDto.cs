using System.ComponentModel.DataAnnotations;

namespace Buoi2_WebAPI.DTOs
{
    // ================= 1. DTO ĐĂNG KÝ HỘI VIÊN MỚI =================
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Email là bắt buộc"), EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username là bắt buộc"), MinLength(3, ErrorMessage = "Username tối thiểu 3 ký tự")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc"), MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc"), Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        public string Gender { get; set; } = "Male"; // Male, Female, Other
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;

        // Thể trạng & Mục tiêu
        public double? HeightCm { get; set; }
        public double? WeightKg { get; set; }
        public string? HealthNotes { get; set; }
        public string? FitnessGoal { get; set; }

        // Gói tập đăng ký: Basic, VIP, Gold, Diamond...
        public string MembershipPackage { get; set; } = "Basic";

        // Liên hệ khẩn cấp
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string EmergencyRelationship { get; set; } = string.Empty;
    }

    // ================= 2. DTO CẬP NHẬT THÔNG TIN HỘI VIÊN =================
    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = "Male";
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }

        public double? HeightCm { get; set; }
        public double? WeightKg { get; set; }
        public string? HealthNotes { get; set; }
        public string? FitnessGoal { get; set; }

        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string EmergencyRelationship { get; set; } = string.Empty;
    }

    // ================= 3. DTO ĐĂNG NHẬP =================
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = string.Empty;
    }

    // ================= 4. DTO TRẢ VỀ THÔNG TIN HỘI VIÊN =================
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Thông tin gói tập Gym
        public string MembershipPackage { get; set; } = string.Empty;
        public string MembershipStatus { get; set; } = string.Empty;
        public DateTime? MembershipEndDate { get; set; }

        // Trạng thái & Ngày tạo
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


