using Microsoft.AspNetCore.Mvc;
using QuanlyPhongtapGymFitnessClub.Data;
using QuanlyPhongtapGymFitnessClub.DTOs;
using QuanlyPhongtapGymFitnessClub.Models;

namespace QuanlyPhongtapGymFitnessClub.Controllers
{
    /// <summary>
    /// API Quan ly San pham Gym (nuoc uong, supplement, phu kien)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        // ------------------------------------------------------------------------
        // READ ALL (GET) - api/products
        // ------------------------------------------------------------------------
        [HttpGet]
        public ActionResult<List<Product>> GetAll(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] bool? inStock)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(s)
                                      || (p.Description != null && p.Description.ToLower().Contains(s)));
            }

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category == category);

            if (inStock.HasValue && inStock.Value)
                query = query.Where(p => p.StockQuantity > 0);

            return Ok(query.OrderBy(p => p.Category).ThenBy(p => p.Name).ToList());
        }

        // ------------------------------------------------------------------------
        // READ BY ID (GET) - api/products/{id}
        // ------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Khong tim thay san pham co Id = {id}" });

            return Ok(product);
        }

        // ------------------------------------------------------------------------
        // CREATE (POST) - api/products
        // ------------------------------------------------------------------------
        [HttpPost]
        public ActionResult<Product> Create([FromBody] ProductCreateDto dto)
        {
            var product = new Product
            {
                Name          = dto.Name,
                Description   = dto.Description,
                Price         = dto.Price,
                StockQuantity = dto.StockQuantity,
                Category      = dto.Category,
                ImageUrl      = dto.ImageUrl,
                IsActive      = true,
                CreatedAt     = DateTime.UtcNow
            };

            _db.Products.Add(product);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // ------------------------------------------------------------------------
        // UPDATE (PUT) - api/products/{id}
        // ------------------------------------------------------------------------
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ProductUpdateDto dto)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Khong tim thay san pham co Id = {id}" });

            if (!string.IsNullOrWhiteSpace(dto.Name))     product.Name          = dto.Name;
            if (dto.Description != null)                  product.Description   = dto.Description;
            if (dto.Price.HasValue)                       product.Price         = dto.Price.Value;
            if (dto.StockQuantity.HasValue)               product.StockQuantity = dto.StockQuantity.Value;
            if (!string.IsNullOrWhiteSpace(dto.Category)) product.Category      = dto.Category;
            if (dto.ImageUrl != null)                     product.ImageUrl      = dto.ImageUrl;
            if (dto.IsActive.HasValue)                    product.IsActive      = dto.IsActive.Value;
            product.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();

            return Ok(new { message = $"Cap nhat thanh cong san pham Id = {id}" });
        }

        // ------------------------------------------------------------------------
        // UPDATE STOCK (PATCH) - api/products/{id}/stock?quantity=10
        // ------------------------------------------------------------------------
        [HttpPatch("{id:int}/stock")]
        public IActionResult UpdateStock(int id, [FromQuery] int quantity)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Khong tim thay san pham co Id = {id}" });

            if (product.StockQuantity + quantity < 0)
                return BadRequest(new { message = $"So luong ton kho khong du. Hien co: {product.StockQuantity}, yeu cau giam: {Math.Abs(quantity)}" });

            var oldStock = product.StockQuantity;
            product.StockQuantity += quantity;
            product.UpdatedAt      = DateTime.UtcNow;
            _db.SaveChanges();

            return Ok(new
            {
                message   = $"Da cap nhat kho san pham '{product.Name}'",
                productId = product.Id,
                oldStock  = oldStock,
                newStock  = product.StockQuantity
            });
        }

        // ------------------------------------------------------------------------
        // DELETE - api/products/{id}
        // ------------------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = $"Khong tim thay san pham co Id = {id}" });

            bool isUsed = _db.InvoiceItems.Any(ii => ii.ProductId == id);
            if (isUsed)
                return BadRequest(new { message = "Khong the xoa san pham da xuat hien trong hoa don. Ban co the cap nhat IsActive = false." });

            _db.Products.Remove(product);
            _db.SaveChanges();

            return Ok(new { message = $"Da xoa san pham Id = {id}" });
        }
    }
}
