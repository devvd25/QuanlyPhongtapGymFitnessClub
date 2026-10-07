using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    // Enum: Danh sach cac goi tap -> Swagger tao Dropdown
    public enum MembershipPackageType
    {
        Basic,
        VIP,
        Diamond
    }

    // ================= 1. DTO DANG KY HOI VIEN MOI =================
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Email la bat buoc")]
        [EmailAddress(ErrorMessage = "Email khong dung dinh dang")]
        [MaxLength(100, ErrorMessage = "Email toi da 100 ky tu")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username la bat buoc")]
        [MinLength(3, ErrorMessage = "Username toi thieu 3 ky tu")]
        [MaxLength(50, ErrorMessage = "Username toi da 50 ky tu")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mat khau la bat buoc")]
        [MinLength(6, ErrorMessage = "Mat khau toi thieu 6 ky tu")]
        [MaxLength(100, ErrorMessage = "Mat khau toi da 100 ky tu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ho va ten la bat buoc")]
        [MaxLength(100, ErrorMessage = "Ho ten toi da 100 ky tu")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "So dien thoai la bat buoc")]
        [Phone(ErrorMessage = "So dien thoai khong hop le")]
        [MaxLength(15, ErrorMessage = "So dien thoai toi da 15 ky tu")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male";

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(200, ErrorMessage = "Dia chi toi da 200 ky tu")]
        public string? Address { get; set; }

        [Range(50, 250, ErrorMessage = "Chieu cao phai tu 50cm den 250cm")]
        public double? HeightCm { get; set; }

        [Range(20, 300, ErrorMessage = "Can nang phai tu 20kg den 300kg")]
        public double? WeightKg { get; set; }

        [MaxLength(500)]
        public string? HealthNotes { get; set; }

        [MaxLength(200)]
        public string? FitnessGoal { get; set; }

        [MaxLength(30)]
        public string MembershipPackage { get; set; } = "Basic";
    }

    // ================= 2. DTO CAP NHAT THONG TIN HOI VIEN =================
    public class UserUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Ho ten toi da 100 ky tu")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email khong dung dinh dang")]
        [MaxLength(100, ErrorMessage = "Email toi da 100 ky tu")]
        public string? Email { get; set; }

        [MaxLength(15, ErrorMessage = "So dien thoai toi da 15 ky tu")]
        public string? Phone { get; set; }

        [MaxLength(30)]
        public string? MembershipPackage { get; set; }

        [MaxLength(20)]
        public string? MembershipStatus { get; set; }

        public bool? IsActive { get; set; }
    }

    // ================= 3. DTO DANG NHAP =================
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username la bat buoc")]
        [MaxLength(50, ErrorMessage = "Username toi da 50 ky tu")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mat khau la bat buoc")]
        [MaxLength(100, ErrorMessage = "Mat khau toi da 100 ky tu")]
        public string Password { get; set; } = string.Empty;
    }

    // ================= 4. DTO TRA VE THONG TIN HOI VIEN =================
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string MembershipPackage { get; set; } = string.Empty;
        public string MembershipStatus { get; set; } = string.Empty;
        public DateTime? MembershipEndDate { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
