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
        [Range(0, 100000000, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }

        [Range(0, 100000, ErrorMessage = "Số lượng tồn kho phải lớn hơn hoặc bằng 0")]
        public int StockQuantity { get; set; } = 0;

        [MaxLength(50)]
        public string Category { get; set; } = "Drink"; // Drink, Supplement, Clothing, Accessory

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }

    // ================= DTO CẬP NHẬT SẢN PHẨM =================
    public class ProductUpdateDto
    {
        [MaxLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, 100000000, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0")]
        public decimal? Price { get; set; }

        [Range(0, 100000, ErrorMessage = "Số lượng tồn kho phải lớn hơn hoặc bằng 0")]
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
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
