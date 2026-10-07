using System.ComponentModel.DataAnnotations;
using QuanlyPhongtapGymFitnessClub.Attributes;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    /// <summary>
    /// Chuyen mon cua Huan luyen vien the hinh
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
    /// DTO tiep nhan du lieu khi them moi Huan luyen vien (Personal Trainer)
    /// </summary>
    public class TrainerCreateDto
    {
        [Required(ErrorMessage = "Username la bat buoc")]
        [MinLength(3, ErrorMessage = "Username phai co it nhat 3 ky tu")]
        [MaxLength(50, ErrorMessage = "Username toi da 50 ky tu")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mat khau ban dau la bat buoc")]
        [MinLength(6, ErrorMessage = "Mat khau phai tu 6 ky tu tro len")]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ho va ten la bat buoc")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email la bat buoc")]
        [EmailAddress(ErrorMessage = "Dinh dang email khong hop le")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "So dien thoai la bat buoc")]
        [Phone(ErrorMessage = "So dien thoai khong hop le")]
        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male";

        /// <summary>
        /// Ngay sinh (Custom Validation: HLV phai tu 18 den 60 tuoi)
        /// </summary>
        [GymAgeValidation(18, 60, ErrorMessage = "Huan luyen vien phai trong do tuoi tu 18 den 60 tuoi")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Chuyen mon giang day la bat buoc (Gym, Fitness, Yoga, Boxing...)")]
        public string Specialty { get; set; } = "Gym";

        [MaxLength(200, ErrorMessage = "Chung chi toi da 200 ky tu")]
        public string Certifications { get; set; } = string.Empty;

        [Range(100000, 5000000, ErrorMessage = "Gia thue PT theo gio phai tu 100,000 VND den 5,000,000 VND")]
        public decimal HourlyRate { get; set; } = 300000;

        /// <summary>
        /// So hoc vien toi da (Custom Validation: phai la so chan de ghep cap luyen tap)
        /// </summary>
        [Range(2, 50, ErrorMessage = "So hoc vien toi da tu 2 den 50 nguoi")]
        [MustBeEven(ErrorMessage = "So luong hoc vien toi da phai la so chan de tien ghep cap luyen tap")]
        public int MaxMembers { get; set; } = 10;
    }

    /// <summary>
    /// DTO cap nhat thong tin Huan luyen vien (tat ca truong la optional)
    /// </summary>
    public class TrainerUpdateDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Dinh dang email khong hop le")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "So dien thoai khong hop le")]
        [MaxLength(15)]
        public string? Phone { get; set; }

        public string? Specialty { get; set; }

        [MaxLength(200)]
        public string? Certifications { get; set; }

        [Range(100000, 5000000, ErrorMessage = "Gia thue PT theo gio phai tu 100,000 VND den 5,000,000 VND")]
        public decimal? HourlyRate { get; set; }

        [Range(2, 50, ErrorMessage = "So hoc vien toi da tu 2 den 50 nguoi")]
        [MustBeEven(ErrorMessage = "So luong hoc vien toi da phai la so chan")]
        public int? MaxMembers { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO thong tin Huan luyen vien tra ve Client (khong chua thong tin nhay cam)
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
