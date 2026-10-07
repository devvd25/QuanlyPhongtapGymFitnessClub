using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    /// <summary>
    /// DTO tao moi san pham
    /// </summary>
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Ten san pham la bat buoc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(100, 100_000_000, ErrorMessage = "Gia phai tu 100d den 100,000,000d")]
        public decimal Price { get; set; }

        [Range(0, 100_000)]
        public int StockQuantity { get; set; } = 0;

        /// <summary>Danh muc: Drink, Supplement, Clothing, Accessory</summary>
        [MaxLength(50)]
        public string Category { get; set; } = "Drink";

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// DTO cap nhat san pham (tat ca truong la optional)
    /// </summary>
    public class ProductUpdateDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(100, 100_000_000)]
        public decimal? Price { get; set; }

        [Range(0, 100_000)]
        public int? StockQuantity { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool? IsActive { get; set; }
    }
}
