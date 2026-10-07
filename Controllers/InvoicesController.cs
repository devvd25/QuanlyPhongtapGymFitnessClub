using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quan ly Hoa don va Ban hang (Invoices)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InvoicesController(AppDbContext db)
        {
            _db = db;
        }

        // ------------------------------------------------------------------------
        // READ ALL (GET) - api/invoices
        // ------------------------------------------------------------------------
        [HttpGet]
        public ActionResult<List<InvoiceResponseDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] int? userId,
            [FromQuery] int? staffId)
        {
            var query = _db.Invoices
                           .Include(i => i.Items)
                           .Include(i => i.User)
                           .Include(i => i.Staff)
                           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(i => i.InvoiceCode.ToLower().Contains(s)
                                      || (i.User != null && i.User.FullName.ToLower().Contains(s))
                                      || i.Staff.FullName.ToLower().Contains(s));
            }

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(i => i.Status == status);

            if (userId.HasValue)
                query = query.Where(i => i.UserId == userId.Value);

            if (staffId.HasValue)
                query = query.Where(i => i.StaffId == staffId.Value);

            var invoices = query.OrderByDescending(i => i.CreatedAt).ToList().Select(i => new InvoiceResponseDto
            {
                Id            = i.Id,
                InvoiceCode   = i.InvoiceCode,
                UserId        = i.UserId,
                UserFullName  = i.User?.FullName,
                StaffId       = i.StaffId,
                StaffFullName = i.Staff?.FullName ?? string.Empty,
                TotalAmount   = i.TotalAmount,
                PaymentMethod = i.PaymentMethod,
                Status        = i.Status,
                Notes         = i.Notes,
                CreatedAt     = i.CreatedAt,
                Items         = i.Items.Select(it => new InvoiceItemResponseDto
                {
                    Id          = it.Id,
                    ProductId   = it.ProductId,
                    ProductName = it.ProductName,
                    Quantity    = it.Quantity,
                    UnitPrice   = it.UnitPrice,
                    SubTotal    = it.SubTotal
                }).ToList()
            }).ToList();

            return Ok(invoices);
        }

        // ------------------------------------------------------------------------
        // READ BY ID (GET) - api/invoices/{id}
        // ------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public ActionResult<InvoiceResponseDto> GetById(int id)
        {
            var i = _db.Invoices
                       .Include(inv => inv.Items)
                       .Include(inv => inv.User)
                       .Include(inv => inv.Staff)
                       .FirstOrDefault(inv => inv.Id == id);

            if (i == null)
                return NotFound(new { message = $"Khong tim thay hoa don co Id = {id}" });

            return Ok(new InvoiceResponseDto
            {
                Id            = i.Id,
                InvoiceCode   = i.InvoiceCode,
                UserId        = i.UserId,
                UserFullName  = i.User?.FullName,
                StaffId       = i.StaffId,
                StaffFullName = i.Staff?.FullName ?? string.Empty,
                TotalAmount   = i.TotalAmount,
                PaymentMethod = i.PaymentMethod,
                Status        = i.Status,
                Notes         = i.Notes,
                CreatedAt     = i.CreatedAt,
                Items         = i.Items.Select(it => new InvoiceItemResponseDto
                {
                    Id          = it.Id,
                    ProductId   = it.ProductId,
                    ProductName = it.ProductName,
                    Quantity    = it.Quantity,
                    UnitPrice   = it.UnitPrice,
                    SubTotal    = it.SubTotal
                }).ToList()
            });
        }

        // ------------------------------------------------------------------------
        // CREATE (POST) - api/invoices
        // ------------------------------------------------------------------------
        [HttpPost]
        public ActionResult<InvoiceResponseDto> Create([FromBody] InvoiceCreateDto dto)
        {
            var staff = _db.Staffs.FirstOrDefault(s => s.Id == dto.StaffId);
            if (staff == null)
                return BadRequest(new { message = $"Nhan vien co Id = {dto.StaffId} khong ton tai." });

            string? userFullName = null;
            if (dto.UserId.HasValue)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == dto.UserId.Value && !u.IsDeleted);
                if (user == null)
                    return BadRequest(new { message = $"Hoi vien co Id = {dto.UserId.Value} khong ton tai." });
                userFullName = user.FullName;
            }

            if (dto.Items == null || dto.Items.Count == 0)
                return BadRequest(new { message = "Hoa don phai co it nhat 1 san pham." });

            var invoiceItems = new List<InvoiceItem>();
            decimal totalAmount = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == itemDto.ProductId);
                if (product == null)
                    return BadRequest(new { message = $"San pham co Id = {itemDto.ProductId} khong ton tai." });

                if (product.StockQuantity < itemDto.Quantity)
                    return BadRequest(new { message = $"San pham '{product.Name}' khong du so luong trong kho (con {product.StockQuantity}, yeu cau {itemDto.Quantity})." });

                // Tru kho san pham
                product.StockQuantity -= itemDto.Quantity;
                product.UpdatedAt = DateTime.UtcNow;

                var subTotal = product.Price * itemDto.Quantity;
                totalAmount += subTotal;

                invoiceItems.Add(new InvoiceItem
                {
                    ProductId   = product.Id,
                    ProductName = product.Name,
                    Quantity    = itemDto.Quantity,
                    UnitPrice   = product.Price,
                    SubTotal    = subTotal
                });
            }

            var invoiceCode = $"INV-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}";

            var invoice = new Invoice
            {
                InvoiceCode   = invoiceCode,
                UserId        = dto.UserId,
                StaffId       = dto.StaffId,
                TotalAmount   = totalAmount,
                PaymentMethod = dto.PaymentMethod ?? "Cash",
                Status        = "Paid",
                Notes         = dto.Notes,
                Items         = invoiceItems,
                CreatedAt     = DateTime.UtcNow
            };

            _db.Invoices.Add(invoice);
            _db.SaveChanges();

            var response = new InvoiceResponseDto
            {
                Id            = invoice.Id,
                InvoiceCode   = invoice.InvoiceCode,
                UserId        = invoice.UserId,
                UserFullName  = userFullName,
                StaffId       = invoice.StaffId,
                StaffFullName = staff.FullName,
                TotalAmount   = invoice.TotalAmount,
                PaymentMethod = invoice.PaymentMethod,
                Status        = invoice.Status,
                Notes         = invoice.Notes,
                CreatedAt     = invoice.CreatedAt,
                Items         = invoice.Items.Select(it => new InvoiceItemResponseDto
                {
                    Id          = it.Id,
                    ProductId   = it.ProductId,
                    ProductName = it.ProductName,
                    Quantity    = it.Quantity,
                    UnitPrice   = it.UnitPrice,
                    SubTotal    = it.SubTotal
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, response);
        }

        // ------------------------------------------------------------------------
        // UPDATE STATUS / NOTES (PUT) - api/invoices/{id}
        // ------------------------------------------------------------------------
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] InvoiceUpdateDto dto)
        {
            var invoice = _db.Invoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
                return NotFound(new { message = $"Khong tim thay hoa don co Id = {id}" });

            if (!string.IsNullOrWhiteSpace(dto.Status))        invoice.Status        = dto.Status;
            if (!string.IsNullOrWhiteSpace(dto.PaymentMethod)) invoice.PaymentMethod = dto.PaymentMethod;
            if (dto.Notes != null)                             invoice.Notes         = dto.Notes;

            _db.SaveChanges();
            return Ok(new { message = $"Cap nhat hoa don Id = {id} thanh cong." });
        }

        // ------------------------------------------------------------------------
        // DELETE - api/invoices/{id}
        // ------------------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var invoice = _db.Invoices.Include(i => i.Items).FirstOrDefault(i => i.Id == id);
            if (invoice == null)
                return NotFound(new { message = $"Khong tim thay hoa don co Id = {id}" });

            _db.Invoices.Remove(invoice);
            _db.SaveChanges();

            return Ok(new { message = $"Da xoa hoa don Id = {id}" });
        }
    }
}
