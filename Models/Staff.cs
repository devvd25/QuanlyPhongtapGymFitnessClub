using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanlyPhongtapGymFitnessClub.Models
{
    /// <summary>
    /// Model Ä‘áº¡i diá»‡n cho NhĂ¢n viĂªn lá»… tĂ¢n / BĂ¡n hĂ ng / CSKH trong phĂ²ng Gym
    /// </summary>
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = "Female";

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MaxLength(30)]
        public string WorkShift { get; set; } = "Morning"; // Morning (06:00 - 14:00), Afternoon (14:00 - 22:00), FullDay

        [Range(1, 20)]
        public int CounterNumber { get; set; } = 1; // Quáº§y lĂ m viá»‡c sá»‘ 1, 2, 3...

        [MaxLength(50)]
        public string Department { get; set; } = "Reception"; // Reception, Sales, CustomerService

        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        public bool IsOnDuty { get; set; } = true; // Äang trong ca lĂ m viá»‡c

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
