using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    // Enum: Danh sách các gói tập -> Swagger tạo Dropdown
    public enum MembershipPackageType
    {
        Basic,
        VIP,
        Diamond
    }

    // ================= 1. DTO ĐĂNG KÝ HỘI VIÊN MỚI =================
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username là bắt buộc")]
        [MinLength(3, ErrorMessage = "Username tối thiểu 3 ký tự")]
        [MaxLength(50, ErrorMessage = "Username tối đa 50 ký tự")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [MaxLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male";

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(200, ErrorMessage = "Địa chỉ tối đa 200 ký tự")]
        public string? Address { get; set; }

        [Range(50, 250, ErrorMessage = "Chiều cao phải từ 50cm đến 250cm")]
        public double? HeightCm { get; set; }

        [Range(20, 300, ErrorMessage = "Cân nặng phải từ 20kg đến 300kg")]
        public double? WeightKg { get; set; }

        [MaxLength(500)]
        public string? HealthNotes { get; set; }

        [MaxLength(200)]
        public string? FitnessGoal { get; set; }

        [MaxLength(30)]
        public string MembershipPackage { get; set; } = "Basic";
    }

    // ================= 2. DTO CẬP NHẬT THÔNG TIN HỘI VIÊN =================
    public class UserUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string? Email { get; set; }

        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string? Phone { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(30)]
        public string? MembershipPackage { get; set; }

        [MaxLength(20)]
        public string? MembershipStatus { get; set; }

        public int? RemainingPtSessions { get; set; }

        public int? AssignedTrainerId { get; set; }

        public bool? IsActive { get; set; }
    }

    // ================= 3. DTO ĐĂNG NHẬP =================
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Username tối đa 50 ký tự")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
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

        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }

        public string MembershipPackage { get; set; } = string.Empty;
        public string MembershipStatus { get; set; } = string.Empty;
        public DateTime? MembershipEndDate { get; set; }

        public int RemainingPtSessions { get; set; }
        public int? AssignedTrainerId { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
