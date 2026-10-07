using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanlyPhongtapGymFitnessClub.Models
{
    /// <summary>
    /// Model đại diện cho Hóa đơn bán hàng trong phòng Gym
    /// Kết nối: Staff (thu ngân) → User (hội viên) → Product (qua InvoiceItems)
    /// </summary>
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Mã hóa đơn duy nhất (VD: INV-20260930-0001)</summary>
        [Required]
        [MaxLength(30)]
        public string InvoiceCode { get; set; } = string.Empty;

        // ── KHÓA NGOẠI ───────────────────────────────────────────────────────

        /// <summary>Hội viên mua hàng (nullable: khách vãng lai không có tài khoản)</summary>
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        /// <summary>Nhân viên tạo hóa đơn (thu ngân)</summary>
        [Required]
        public int StaffId { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; } = null!;

        // ── THÔNG TIN HÓA ĐƠN ────────────────────────────────────────────────

        /// <summary>Tổng tiền hóa đơn (tính từ các InvoiceItems)</summary>
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; } = 0;

        /// <summary>Phương thức thanh toán: Cash, Card, Transfer</summary>
        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; } = "Cash";

        /// <summary>Trạng thái: Pending, Paid, Cancelled</summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Paid";

        [MaxLength(500)]
        public string? Notes { get; set; }

        /// <summary>Danh sách dòng sản phẩm trong hóa đơn này</summary>
        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Model đại diện cho 1 dòng sản phẩm trong Hóa đơn
    /// Ví dụ: "2 lon Monster Energy × 35,000đ = 70,000đ"
    /// </summary>
    public class InvoiceItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // ── KHÓA NGOẠI ───────────────────────────────────────────────────────

        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        // ── THÔNG TIN DÒNG HÀNG ───────────────────────────────────────────────

        /// <summary>Tên sản phẩm lưu cố định tại thời điểm mua (snapshot)</summary>
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; } = 1;

        /// <summary>Đơn giá tại thời điểm mua (snapshot, không đổi dù Product.Price thay đổi sau)</summary>
        [Required]
        public decimal UnitPrice { get; set; }

        /// <summary>Thành tiền = Quantity × UnitPrice</summary>
        [Required]
        public decimal SubTotal { get; set; }
    }
}
