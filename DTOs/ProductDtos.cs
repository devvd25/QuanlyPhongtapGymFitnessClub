using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    // ================= DTO TẠO SẢN PHẨM MỚI =================
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0, 100_000_000, ErrorMessage = "Giá sản phẩm phải từ 0đ đến 100,000,000đ")]
        public decimal Price { get; set; }

        [Range(0, 100_000, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 100,000")]
        public int StockQuantity { get; set; } = 0;

        /// <summary>Danh mục: Drink, Supplement, Clothing, Accessory</summary>
        [MaxLength(50)]
        public string Category { get; set; } = "Drink";

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }

    // ================= DTO CẬP NHẬT SẢN PHẨM =================
    public class ProductUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
        public string? Name { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? Description { get; set; }

        [Range(0, 100_000_000, ErrorMessage = "Giá sản phẩm phải từ 0đ đến 100,000,000đ")]
        public decimal? Price { get; set; }

        [Range(0, 100_000, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 100,000")]
        public int? StockQuantity { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool? IsActive { get; set; }
    }

    // ================= DTO TRẢ VỀ DỮ LIỆU SẢN PHẨM =================
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int? ManagedByStaffId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
