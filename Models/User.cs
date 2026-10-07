using System.ComponentModel.DataAnnotations;      // ThÆ° viá»‡n chá»©a cĂ¡c Attribute validation: [Required], [MaxLength], [EmailAddress]...
using System.ComponentModel.DataAnnotations.Schema; // ThÆ° viá»‡n chá»©a [Key], [DatabaseGenerated] Ä‘á»ƒ cáº¥u hĂ¬nh mapping vá»›i Database

namespace QuanlyPhongtapGymFitnessClub.Models
{
    public class User
    {
        // ================= 1. KHĂ“A CHĂNH & TĂ€I KHOáº¢N ÄÄ‚NG NHáº¬P =================

        [Key] // ÄĂ¡nh dáº¥u Ä‘Ă¢y lĂ  khĂ³a chĂ­nh (Primary Key)
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database tá»± tÄƒng Id (1, 2, 3...)
        public int Id { get; set; }

        [Required] // Báº¯t buá»™c nháº­p, khĂ´ng Ä‘Æ°á»£c null
        [MaxLength(50)] // Giá»›i háº¡n tá»‘i Ä‘a 50 kĂ½ tá»±, trĂ¡nh táº¡o cá»™t nvarchar(MAX) trong SQL
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress] // Validate Ä‘Ăºng Ä‘á»‹nh dáº¡ng email (pháº£i cĂ³ @)
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // Chuá»—i ngáº«u nhiĂªn dĂ¹ng Ä‘á»ƒ mĂ£ hĂ³a máº­t kháº©u

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); // Máº­t kháº©u Ä‘Ă£ Ä‘Æ°á»£c mĂ£ hĂ³a (khĂ´ng lÆ°u plain text)

        [MaxLength(500)]
        public string? RefreshToken { get; set; } // Token lĂ m má»›i phiĂªn Ä‘Äƒng nháº­p
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // ================= 2. THĂ”NG TIN CĂ NHĂ‚N =================

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone] // Validate Ä‘á»‹nh dáº¡ng sá»‘ Ä‘iá»‡n thoáº¡i
        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male"; // Male, Female, Other

        public DateTime? DateOfBirth { get; set; } // DateTime? = nullable, cho phĂ©p null

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? AvatarUrl { get; set; } // string? = nullable, khĂ´ng báº¯t buá»™c

        // ================= 3. THĂ”NG TIN THá»‚ TRáº NG & Sá»¨C KHá»E GYM =================

        [Range(50, 250, ErrorMessage = "Chiá»u cao pháº£i tá»« 50cm Ä‘áº¿n 250cm")] // Giá»›i háº¡n giĂ¡ trá»‹ sá»‘ trong khoáº£ng
        public double? HeightCm { get; set; }

        [Range(20, 300, ErrorMessage = "CĂ¢n náº·ng pháº£i tá»« 20kg Ä‘áº¿n 300kg")]
        public double? WeightKg { get; set; }

        [MaxLength(500)]
        public string? HealthNotes { get; set; }

        [MaxLength(200)]
        public string? FitnessGoal { get; set; } // Má»¥c tiĂªu táº­p: Giáº£m má»¡, TÄƒng cÆ¡...

        // ================= 4. THĂ”NG TIN GĂ“I Táº¬P & THáºº Há»˜I VIĂN =================

        [MaxLength(50)]
        public string? MemberCardCode { get; set; } // MĂ£ tháº» tá»« / QR Code Ä‘á»ƒ check-in

        [Required]
        [MaxLength(30)]
        public string MembershipPackage { get; set; } = "Basic"; // Basic, VIP, Diamond

        [Required]
        [MaxLength(20)]
        public string MembershipStatus { get; set; } = "Active"; // Active, Expired, Suspended

        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }

        [Range(0, 1000)] // Sá»‘ buá»•i PT cĂ²n láº¡i: tá»« 0 Ä‘áº¿n 1000
        public int RemainingPtSessions { get; set; } = 0;

        // int? (nullable): cho phĂ©p null vĂ¬ há»™i viĂªn táº­p gĂ³i tá»± do/khĂ´ng thuĂª PT thĂ¬ trÆ°á»ng nĂ y sáº½ Ä‘á»ƒ trá»‘ng.
        public int? AssignedTrainerId { get; set; }

        [ForeignKey("AssignedTrainerId")]
        public virtual Trainer? AssignedTrainer { get; set; }

        // ================= 5. QUáº¢N LĂ TRáº NG THĂI & AUDIT LOGS =================
        public bool IsActive { get; set; } = true; // true = hoáº¡t Ä‘á»™ng, false = khĂ³a tĂ i khoáº£n
        public bool IsDeleted { get; set; } = false; // XĂ³a má»m (Soft Delete)

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
