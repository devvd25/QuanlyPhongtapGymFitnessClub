using System.ComponentModel.DataAnnotations;
using Buoi2_WebAPI.Attributes;

namespace Buoi2_WebAPI.DTOs
{
    /// <summary>
    /// Chuyên môn của Huấn luyện viên thể hình
    /// </summary>
    public enum TrainerSpecialty
    {
        Gym,
        Fitness,
        Bodybuilding,
        Yoga,
        Boxing,
        Pilates
    }

    /// <summary>
    /// DTO tiếp nhận dữ liệu khi thêm mới Huấn luyện viên (Personal Trainer)
    /// </summary>
    public class TrainerCreateDto
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        [MinLength(3, ErrorMessage = "Username phải có ít nhất 3 ký tự")]
        [MaxLength(50, ErrorMessage = "Username tối đa 50 ký tự")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu ban đầu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        [MaxLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male";

        /// <summary>
        /// Ngày sinh (Sử dụng Custom Validation [GymAgeValidation] để đảm bảo tuổi lao động từ 18 đến 60)
        /// </summary>
        [GymAgeValidation(18, 60, ErrorMessage = "Huấn luyện viên phải trong độ tuổi từ 18 đến 60 tuổi")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Chuyên môn giảng dạy là bắt buộc (Gym, Fitness, Yoga, Boxing...)")]
        public string Specialty { get; set; } = "Gym";

        [Range(0, 40, ErrorMessage = "Kinh nghiệm phải từ 0 đến 40 năm")]
        public int ExperienceYears { get; set; } = 1;

        [MaxLength(200, ErrorMessage = "Chứng chỉ tối đa 200 ký tự")]
        public string Certifications { get; set; } = string.Empty;

        [Range(100000, 5000000, ErrorMessage = "Giá thuê PT theo giờ phải từ 100,000 VND đến 5,000,000 VND")]
        public decimal HourlyRate { get; set; } = 300000;

        /// <summary>
        /// Số học viên tối đa (Áp dụng Custom Validation [MustBeEven] theo yêu cầu Buổi 4 slide 18)
        /// </summary>
        [Range(2, 50, ErrorMessage = "Số học viên tối đa từ 2 đến 50 người")]
        [MustBeEven(ErrorMessage = "Số lượng học viên tối đa phải là số chẵn để tiện ghép cặp luyện tập")]
        public int MaxMembers { get; set; } = 10;
    }

    /// <summary>
    /// DTO cập nhật thông tin Huấn luyện viên
    /// </summary>
    public class TrainerUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        [MaxLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string? Phone { get; set; }

        public string? Specialty { get; set; }

        [Range(0, 40, ErrorMessage = "Kinh nghiệm phải từ 0 đến 40 năm")]
        public int? ExperienceYears { get; set; }

        [MaxLength(200, ErrorMessage = "Chứng chỉ tối đa 200 ký tự")]
        public string? Certifications { get; set; }

        [Range(100000, 5000000, ErrorMessage = "Giá thuê PT theo giờ phải từ 100,000 VND đến 5,000,000 VND")]
        public decimal? HourlyRate { get; set; }

        [Range(2, 50, ErrorMessage = "Số học viên tối đa từ 2 đến 50 người")]
        [MustBeEven(ErrorMessage = "Số lượng học viên tối đa phải là số chẵn")]
        public int? MaxMembers { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO thông tin Huấn luyện viên trả về Client (Không chứa thông tin nhạy cảm)
    /// </summary>
    public class TrainerResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public string Certifications { get; set; } = string.Empty;
        public double Rating { get; set; }
        public decimal HourlyRate { get; set; }
        public int MaxMembers { get; set; }
        public int CurrentMembersCount { get; set; }
        public List<int> AssignedMemberIds { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
