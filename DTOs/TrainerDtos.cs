using System.ComponentModel.DataAnnotations;
using QuanlyPhongtapGymFitnessClub.Attributes;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    /// <summary>
    /// ChuyĂªn mĂ´n cá»§a Huáº¥n luyá»‡n viĂªn thá»ƒ hĂ¬nh
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
    /// DTO tiáº¿p nháº­n dá»¯ liá»‡u khi thĂªm má»›i Huáº¥n luyá»‡n viĂªn (Personal Trainer)
    /// </summary>
    public class TrainerCreateDto
    {
        [Required(ErrorMessage = "Username lĂ  báº¯t buá»™c")]
        [MinLength(3, ErrorMessage = "Username pháº£i cĂ³ Ă­t nháº¥t 3 kĂ½ tá»±")]
        [MaxLength(50, ErrorMessage = "Username tá»‘i Ä‘a 50 kĂ½ tá»±")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Máº­t kháº©u ban Ä‘áº§u lĂ  báº¯t buá»™c")]
        [MinLength(6, ErrorMessage = "Máº­t kháº©u pháº£i tá»« 6 kĂ½ tá»± trá»Ÿ lĂªn")]
        [MaxLength(100, ErrorMessage = "Máº­t kháº©u tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Há» vĂ  tĂªn lĂ  báº¯t buá»™c")]
        [MaxLength(100, ErrorMessage = "Há» vĂ  tĂªn tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email lĂ  báº¯t buá»™c")]
        [EmailAddress(ErrorMessage = "Äá»‹nh dáº¡ng email khĂ´ng há»£p lá»‡")]
        [MaxLength(100, ErrorMessage = "Email tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i lĂ  báº¯t buá»™c")]
        [Phone(ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i khĂ´ng há»£p lá»‡")]
        [MaxLength(15, ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i tá»‘i Ä‘a 15 kĂ½ tá»±")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Male";

        /// <summary>
        /// NgĂ y sinh (Sá»­ dá»¥ng Custom Validation [GymAgeValidation] Ä‘á»ƒ Ä‘áº£m báº£o tuá»•i lao Ä‘á»™ng tá»« 18 Ä‘áº¿n 60)
        /// </summary>
        [GymAgeValidation(18, 60, ErrorMessage = "Huáº¥n luyá»‡n viĂªn pháº£i trong Ä‘á»™ tuá»•i tá»« 18 Ä‘áº¿n 60 tuá»•i")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "ChuyĂªn mĂ´n giáº£ng dáº¡y lĂ  báº¯t buá»™c (Gym, Fitness, Yoga, Boxing...)")]
        public string Specialty { get; set; } = "Gym";

        [Range(0, 40, ErrorMessage = "Kinh nghiá»‡m pháº£i tá»« 0 Ä‘áº¿n 40 nÄƒm")]
        public int ExperienceYears { get; set; } = 1;

        [MaxLength(200, ErrorMessage = "Chá»©ng chá»‰ tá»‘i Ä‘a 200 kĂ½ tá»±")]
        public string Certifications { get; set; } = string.Empty;

        [Range(100000, 5000000, ErrorMessage = "GiĂ¡ thuĂª PT theo giá» pháº£i tá»« 100,000 VND Ä‘áº¿n 5,000,000 VND")]
        public decimal HourlyRate { get; set; } = 300000;

        /// <summary>
        /// Sá»‘ há»c viĂªn tá»‘i Ä‘a (Ăp dá»¥ng Custom Validation [MustBeEven] theo yĂªu cáº§u Buá»•i 4 slide 18)
        /// </summary>
        [Range(2, 50, ErrorMessage = "Sá»‘ há»c viĂªn tá»‘i Ä‘a tá»« 2 Ä‘áº¿n 50 ngÆ°á»i")]
        [MustBeEven(ErrorMessage = "Sá»‘ lÆ°á»£ng há»c viĂªn tá»‘i Ä‘a pháº£i lĂ  sá»‘ cháºµn Ä‘á»ƒ tiá»‡n ghĂ©p cáº·p luyá»‡n táº­p")]
        public int MaxMembers { get; set; } = 10;
    }

    /// <summary>
    /// DTO cáº­p nháº­t thĂ´ng tin Huáº¥n luyá»‡n viĂªn
    /// </summary>
    public class TrainerUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Há» vĂ  tĂªn tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Äá»‹nh dáº¡ng email khĂ´ng há»£p lá»‡")]
        [MaxLength(100, ErrorMessage = "Email tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i khĂ´ng há»£p lá»‡")]
        [MaxLength(15, ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i tá»‘i Ä‘a 15 kĂ½ tá»±")]
        public string? Phone { get; set; }

        public string? Specialty { get; set; }

        [Range(0, 40, ErrorMessage = "Kinh nghiá»‡m pháº£i tá»« 0 Ä‘áº¿n 40 nÄƒm")]
        public int? ExperienceYears { get; set; }

        [MaxLength(200, ErrorMessage = "Chá»©ng chá»‰ tá»‘i Ä‘a 200 kĂ½ tá»±")]
        public string? Certifications { get; set; }

        [Range(100000, 5000000, ErrorMessage = "GiĂ¡ thuĂª PT theo giá» pháº£i tá»« 100,000 VND Ä‘áº¿n 5,000,000 VND")]
        public decimal? HourlyRate { get; set; }

        [Range(2, 50, ErrorMessage = "Sá»‘ há»c viĂªn tá»‘i Ä‘a tá»« 2 Ä‘áº¿n 50 ngÆ°á»i")]
        [MustBeEven(ErrorMessage = "Sá»‘ lÆ°á»£ng há»c viĂªn tá»‘i Ä‘a pháº£i lĂ  sá»‘ cháºµn")]
        public int? MaxMembers { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO thĂ´ng tin Huáº¥n luyá»‡n viĂªn tráº£ vá» Client (KhĂ´ng chá»©a thĂ´ng tin nháº¡y cáº£m)
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
