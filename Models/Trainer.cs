using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanlyPhongtapGymFitnessClub.Models
{
    /// <summary>
    /// Model Ă„â€˜Ă¡ÂºÂ¡i diĂ¡Â»â€¡n cho HuĂ¡ÂºÂ¥n luyĂ¡Â»â€¡n viÄ‚Âªn cÄ‚Â¡ nhÄ‚Â¢n (Personal Trainer) trong phÄ‚Â²ng Gym
    /// </summary>
    public class Trainer
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
        public string Gender { get; set; } = "Male";

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string Specialty { get; set; } = "Gym"; // Gym, Fitness, Bodybuilding, Yoga, Boxing, Pilates

        [MaxLength(200)]
        public string Certifications { get; set; } = string.Empty; // NASM, ISSA, ACE...

        [Range(1.0, 5.0)]
        public double Rating { get; set; } = 5.0;

        [Range(0, 10000000)]
        public decimal HourlyRate { get; set; } = 300000; // GiÄ‚Â¡ thuÄ‚Âª PT / giĂ¡Â»Â (VND)

        public int MaxMembers { get; set; } = 10;

        // Relational properties
        public virtual ICollection<User> AssignedMembers { get; set; } = new List<User>();

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
