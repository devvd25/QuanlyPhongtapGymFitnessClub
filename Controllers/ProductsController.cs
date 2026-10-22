using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quản lý Sản phẩm / Dịch vụ phòng Gym (Nước uống, Thực phẩm bổ sung, Phụ kiện,...)
    /// Áp dụng CRUD với Entity Framework Core, LINQ và async/await
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------------------------
        // READ ALL (GET) - Lấy danh sách sản phẩm (hỗ trợ tìm kiếm & lọc)
        // ------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] bool? inStock,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchLower) ||
                                         (p.Description != null && p.Description.ToLower().Contains(searchLower)));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (inStock.HasValue && inStock.Value)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var products = await query
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    ManagedByStaffId = p.ManagedByStaffId,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(products);
        }

        // ------------------------------------------------------------------------
        // READ BY ID (GET) - Lấy thông tin sản phẩm theo Id
        // ------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    ManagedByStaffId = p.ManagedByStaffId,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
            }

            return Ok(product);
        }

        // ------------------------------------------------------------------------
        // CREATE (POST) - Thêm mới sản phẩm phòng tập (Chỉ Admin)
        // ------------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] ProductCreateDto request)
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Category = string.IsNullOrWhiteSpace(request.Category) ? "Drink" : request.Category,
                ImageUrl = request.ImageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            var response = MapToDto(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // ------------------------------------------------------------------------
        // UPDATE (PUT) - Cập nhật thông tin sản phẩm (Chỉ Admin)
        // ------------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> Update(int id, [FromBody] ProductUpdateDto request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
            }

            if (!string.IsNullOrWhiteSpace(request.Name)) product.Name = request.Name;
            if (request.Description != null) product.Description = request.Description;
            if (request.Price.HasValue) product.Price = request.Price.Value;
            if (request.StockQuantity.HasValue) product.StockQuantity = request.StockQuantity.Value;
            if (!string.IsNullOrWhiteSpace(request.Category)) product.Category = request.Category;
            if (request.ImageUrl != null) product.ImageUrl = request.ImageUrl;
            if (request.IsActive.HasValue) product.IsActive = request.IsActive.Value;

            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(product));
        }

        // ------------------------------------------------------------------------
        // UPDATE STOCK (PATCH) - api/products/{id}/stock?quantity=10 (Chỉ Admin)
        // ------------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
            }

            if (product.StockQuantity + quantity < 0)
            {
                return BadRequest(new { message = $"Số lượng tồn kho không đủ. Hiện có: {product.StockQuantity}, yêu cầu giảm: {Math.Abs(quantity)}" });
            }

            var oldStock = product.StockQuantity;
            product.StockQuantity += quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Đã cập nhật kho sản phẩm '{product.Name}'",
                productId = product.Id,
                oldStock = oldStock,
                newStock = product.StockQuantity
            });
        }

        // ------------------------------------------------------------------------
        // DELETE (DELETE) - Xóa sản phẩm (Chỉ Admin)
        // ------------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
            }

            bool isUsed = await _context.InvoiceItems.AnyAsync(ii => ii.ProductId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Không thể xóa sản phẩm đã xuất hiện trong hóa đơn. Bạn có thể cập nhật IsActive = false." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã xóa sản phẩm '{product.Name}' (Id = {id}) thành công" });
        }

        // Helper Map DTO
        private static ProductResponseDto MapToDto(Product p)
        {
            return new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category,
                ImageUrl = p.ImageUrl,
                ManagedByStaffId = p.ManagedByStaffId,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            };
        }
    }
}
