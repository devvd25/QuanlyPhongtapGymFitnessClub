using System.ComponentModel.DataAnnotations;
using QuanlyPhongtapGymFitnessClub.Attributes;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    /// <summary>
    /// Ca lĂ m viá»‡c cá»§a nhĂ¢n viĂªn phĂ²ng Gym
    /// </summary>
    public enum WorkShiftType
    {
        Morning,    // 06:00 - 14:00
        Afternoon,  // 14:00 - 22:00
        Evening,    // 17:00 - 22:00
        FullDay     // ToĂ n thá»i gian
    }

    /// <summary>
    /// DTO tiáº¿p nháº­n dá»¯ liá»‡u khi thĂªm má»›i nhĂ¢n viĂªn phĂ²ng Gym
    /// </summary>
    public class StaffCreateDto
    {
        [Required(ErrorMessage = "Username lĂ  báº¯t buá»™c")]
        [MinLength(3, ErrorMessage = "Username pháº£i tá»« 3 kĂ½ tá»± trá»Ÿ lĂªn")]
        [MaxLength(50, ErrorMessage = "Username tá»‘i Ä‘a 50 kĂ½ tá»±")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Máº­t kháº©u lĂ  báº¯t buá»™c")]
        [MinLength(6, ErrorMessage = "Máº­t kháº©u pháº£i tá»« 6 kĂ½ tá»± trá»Ÿ lĂªn")]
        [MaxLength(100, ErrorMessage = "Máº­t kháº©u tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Há» vĂ  tĂªn lĂ  báº¯t buá»™c")]
        [MaxLength(100, ErrorMessage = "Há» vĂ  tĂªn tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email lĂ  báº¯t buá»™c")]
        [EmailAddress(ErrorMessage = "Email khĂ´ng Ä‘Ăºng Ä‘á»‹nh dáº¡ng")]
        [MaxLength(100, ErrorMessage = "Email tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i lĂ  báº¯t buá»™c")]
        [Phone(ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i khĂ´ng há»£p lá»‡")]
        [MaxLength(15, ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i tá»‘i Ä‘a 15 kĂ½ tá»±")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Female";

        /// <summary>
        /// NgĂ y sinh (Sá»­ dá»¥ng Custom Validation [GymAgeValidation])
        /// </summary>
        [GymAgeValidation(18, 65, ErrorMessage = "NhĂ¢n viĂªn pháº£i trong Ä‘á»™ tuá»•i lao Ä‘á»™ng tá»« 18 Ä‘áº¿n 65 tuá»•i")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Ca lĂ m viá»‡c lĂ  báº¯t buá»™c (Morning, Afternoon, Evening, FullDay)")]
        public string WorkShift { get; set; } = "Morning";

        [Range(1, 20, ErrorMessage = "Sá»‘ quáº§y lĂ m viá»‡c pháº£i tá»« 1 Ä‘áº¿n 20")]
        public int CounterNumber { get; set; } = 1;

        [MaxLength(50)]
        public string Department { get; set; } = "Reception";
    }

    /// <summary>
    /// DTO cáº­p nháº­t thĂ´ng tin nhĂ¢n viĂªn phĂ²ng gym
    /// </summary>
    public class StaffUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Há» vĂ  tĂªn tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email khĂ´ng Ä‘Ăºng Ä‘á»‹nh dáº¡ng")]
        [MaxLength(100, ErrorMessage = "Email tá»‘i Ä‘a 100 kĂ½ tá»±")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i khĂ´ng há»£p lá»‡")]
        [MaxLength(15, ErrorMessage = "Sá»‘ Ä‘iá»‡n thoáº¡i tá»‘i Ä‘a 15 kĂ½ tá»±")]
        public string? Phone { get; set; }

        public string? WorkShift { get; set; }

        [Range(1, 20, ErrorMessage = "Sá»‘ quáº§y lĂ m viá»‡c pháº£i tá»« 1 Ä‘áº¿n 20")]
        public int? CounterNumber { get; set; }

        [MaxLength(50)]
        public string? Department { get; set; }

        public bool? IsOnDuty { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO tráº£ vá» thĂ´ng tin nhĂ¢n viĂªn cho Client
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
