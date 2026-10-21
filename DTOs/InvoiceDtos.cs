using System.ComponentModel.DataAnnotations;

namespace QuanlyPhongtapGymFitnessClub.DTOs
{
    // ──────────────────────────────────────────────────────────────────────────
    // DTO TẠO HÓA ĐƠN MỚI
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// DTO tiếp nhận khi tạo hóa đơn mới
    /// </summary>
    public class InvoiceCreateDto
    {
        /// <summary>ID hội viên mua hàng (để null nếu là khách vãng lai)</summary>
        public int? UserId { get; set; }

        /// <summary>ID nhân viên tạo hóa đơn (bắt buộc)</summary>
        [Required(ErrorMessage = "StaffId là bắt buộc")]
        public int StaffId { get; set; }

        /// <summary>Phương thức thanh toán: Cash, Card, Transfer</summary>
        [MaxLength(20)]
        public string PaymentMethod { get; set; } = "Cash";

        [MaxLength(500)]
        public string? Notes { get; set; }

        /// <summary>Danh sách sản phẩm trong hóa đơn (ít nhất 1 sản phẩm)</summary>
        [Required(ErrorMessage = "Hóa đơn phải có ít nhất 1 sản phẩm")]
        [MinLength(1, ErrorMessage = "Hóa đơn phải có ít nhất 1 sản phẩm")]
        public List<InvoiceItemCreateDto> Items { get; set; } = new();
    }

    /// <summary>
    /// DTO chi tiết 1 dòng sản phẩm khi tạo hóa đơn
    /// </summary>
    public class InvoiceItemCreateDto
    {
        [Required(ErrorMessage = "ProductId là bắt buộc")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, 10000, ErrorMessage = "Số lượng phải từ 1 đến 10,000")]
        public int Quantity { get; set; } = 1;
    }

    // ──────────────────────────────────────────────────────────────────────────
    // DTO TRẢ VỀ HÓA ĐƠN
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// DTO trả về thông tin hóa đơn cho Client
    /// </summary>
    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public string InvoiceCode { get; set; } = string.Empty;

        // Thông tin hội viên
        public int? UserId { get; set; }
        public string? UserFullName { get; set; }

        // Thông tin nhân viên thu ngân
        public int StaffId { get; set; }
        public string StaffFullName { get; set; } = string.Empty;

        // Thông tin thanh toán
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        // Danh sách sản phẩm
        public List<InvoiceItemResponseDto> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO trả về chi tiết 1 dòng sản phẩm trong hóa đơn
    /// </summary>
    public class InvoiceItemResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // DTO CẬP NHẬT HÓA ĐƠN
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// DTO cập nhật trạng thái hóa đơn (chỉ cho phép đổi Status và Notes)
    /// </summary>
    public class InvoiceUpdateDto
    {
        /// <summary>Trạng thái mới: Pending, Paid, Cancelled</summary>
        [MaxLength(20)]
        public string? Status { get; set; }

        [MaxLength(20)]
        public string? PaymentMethod { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
