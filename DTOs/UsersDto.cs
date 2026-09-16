using System.ComponentModel.DataAnnotations; // Thư viện validation: [Required], [MaxLength], [MinLength], [EmailAddress], [Phone], [Range]

namespace Buoi2_WebAPI.DTOs
{
    // Enum: Danh sách cố định các vai trò → Swagger tự tạo Dropdown chọn
    public enum UserRole
    {
        Admin,
        Member,
        Trainer
    }

    // Enum: Danh sách cố định các gói tập → Swagger tự tạo Dropdown chọn
    public enum MembershipPackageType
    {
        Basic,
        VIP,
        Diamond
    }

    // ================= 1. DTO ĐĂNG KÝ HỘI VIÊN MỚI =================
    // Dữ liệu Client gửi lên khi đăng ký tài khoản mới
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]       // Không được bỏ trống
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")] // Phải có dấu @
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]  // Giới hạn độ dài
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username là bắt buộc")]
        [MinLength(3, ErrorMessage = "Username tối thiểu 3 ký tự")] // Tối thiểu 3 ký tự
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
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")] // Validate format SĐT
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male";

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(200, ErrorMessage = "Địa chỉ tối đa 200 ký tự")]
        public string Address { get; set; } = string.Empty;

        [Range(50, 250, ErrorMessage = "Chiều cao phải từ 50cm đến 250cm")] // Giới hạn khoảng giá trị số
        public double? HeightCm { get; set; }

        [Range(20, 300, ErrorMessage = "Cân nặng phải từ 20kg đến 300kg")]
        public double? WeightKg { get; set; }

        [MaxLength(500)]
        public string? HealthNotes { get; set; }

        [MaxLength(200)]
        public string? FitnessGoal { get; set; }

        [MaxLength(30)]
        public string MembershipPackage { get; set; } = "Basic";

        // Liên hệ khẩn cấp
        [MaxLength(100)]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "SĐT liên hệ khẩn cấp không hợp lệ")]
        [MaxLength(15)]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        [MaxLength(50)]
        public string EmergencyRelationship { get; set; } = string.Empty;
    }

    // ================= 2. DTO CẬP NHẬT THÔNG TIN HỘI VIÊN =================
    // Dữ liệu Client gửi lên khi sửa thông tin (không có [Required] vì cho phép sửa từng trường)
    public class UserUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = "Member";

        [MaxLength(30)]
        public string MembershipPackage { get; set; } = "Basic";

        [MaxLength(20)]
        public string MembershipStatus { get; set; } = "Active";

        public bool IsActive { get; set; } = true;
    }

    // ================= 3. DTO ĐĂNG NHẬP =================
    // Chỉ cần 2 trường: username + password
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
    // Dữ liệu Server trả về cho Client (không chứa password để bảo mật)
    // Không cần validation vì đây là dữ liệu đi ra, không phải dữ liệu đi vào
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string MembershipPackage { get; set; } = string.Empty;
        public string MembershipStatus { get; set; } = string.Empty;
        public DateTime? MembershipEndDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
