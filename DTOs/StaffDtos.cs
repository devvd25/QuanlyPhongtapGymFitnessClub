using System.ComponentModel.DataAnnotations;
using Buoi2_WebAPI.Attributes;

namespace Buoi2_WebAPI.DTOs
{
    /// <summary>
    /// Ca làm việc của nhân viên phòng Gym
    /// </summary>
    public enum WorkShiftType
    {
        Morning,    // 06:00 - 14:00
        Afternoon,  // 14:00 - 22:00
        Evening,    // 17:00 - 22:00
        FullDay     // Toàn thời gian
    }

    /// <summary>
    /// DTO tiếp nhận dữ liệu khi thêm mới nhân viên phòng Gym
    /// </summary>
    public class StaffCreateDto
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        [MinLength(3, ErrorMessage = "Username phải từ 3 ký tự trở lên")]
        [MaxLength(50, ErrorMessage = "Username tối đa 50 ký tự")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        [MaxLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Female";

        /// <summary>
        /// Ngày sinh (Sử dụng Custom Validation [GymAgeValidation])
        /// </summary>
        [GymAgeValidation(18, 65, ErrorMessage = "Nhân viên phải trong độ tuổi lao động từ 18 đến 65 tuổi")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Ca làm việc là bắt buộc (Morning, Afternoon, Evening, FullDay)")]
        public string WorkShift { get; set; } = "Morning";

        [Range(1, 20, ErrorMessage = "Số quầy làm việc phải từ 1 đến 20")]
        public int CounterNumber { get; set; } = 1;

        [MaxLength(50)]
        public string Department { get; set; } = "Reception";
    }

    /// <summary>
    /// DTO cập nhật thông tin nhân viên phòng gym
    /// </summary>
    public class StaffUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string? Phone { get; set; }

        public string? WorkShift { get; set; }

        [Range(1, 20, ErrorMessage = "Số quầy làm việc phải từ 1 đến 20")]
        public int? CounterNumber { get; set; }

        [MaxLength(50)]
        public string? Department { get; set; }

        public bool? IsOnDuty { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO trả về thông tin nhân viên cho Client
    /// </summary>
    public class StaffResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string WorkShift { get; set; } = string.Empty;
        public int CounterNumber { get; set; }
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public bool IsOnDuty { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
