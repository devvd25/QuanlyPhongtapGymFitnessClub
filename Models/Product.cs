using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanlyPhongtapGymFitnessClub.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; } = 0;

        [MaxLength(50)]
        public string Category { get; set; } = "Drink"; // Drink, Supplement, Clothing, Accessory

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int? ManagedByStaffId { get; set; }

        [ForeignKey("ManagedByStaffId")]
        public virtual Staff? ManagedByStaff { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}