using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quản lý Sản phẩm / Dịch vụ phòng Gym (Nước uống, Thực phẩm bổ sung, Phụ kiện,...)
    /// Áp dụng CRUD với Entity Framework Core, LINQ và async/await (Buổi 6)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        // 1. Khai báo biến DbContext
        private readonly AppDbContext _context;

        // 2. Inject AppDbContext thông qua Constructor
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

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    Category = p.Category,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return Ok(products);
        }

        // ------------------------------------------------------------------------
        // READ BY ID (GET) - Lấy thông tin chi tiết một sản phẩm theo Id
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
        // CREATE (POST) - Thêm mới sản phẩm phòng tập
        // ------------------------------------------------------------------------
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
        // UPDATE (PUT) - Cập nhật thông tin sản phẩm
        // ------------------------------------------------------------------------
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
        // DELETE (DELETE) - Xóa sản phẩm
        // ------------------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm có Id = {id}" });
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
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            };
        }
    }
}