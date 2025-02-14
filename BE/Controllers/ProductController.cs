using Microsoft.AspNetCore.Mvc;
using BE.Models;
using BE.Repositories;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _ProductRepo;

        public ProductController(IProductRepository repo)
        {
            _ProductRepo = repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var Product = await _ProductRepo.GetProductAsync(id);
            return Product == null ? NotFound(new { message = $"Không tìm thấy sản phẩm với ID: {id}" }) : Ok(Product);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(ProductModel model)
        {
            try
            {
                var newProductId = await _ProductRepo.AddProductAsync(model);
                var Product = await _ProductRepo.GetProductAsync(newProductId);
                return Product == null ? NotFound() : Ok(new { message = "Đã thêm thông tin sản phẩm thành công.", data = Product });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            await _ProductRepo.UpdateProductAsync(id, model);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _ProductRepo.DeleteProductAsync(id);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> SearchProducts(
        [FromQuery] string? name,
        [FromQuery] string? description,
        [FromQuery] string? sortByPrice,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var products = await _ProductRepo.GetAllProductsAsync();
            var result = products.AsEnumerable();

            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(description))
            {
                result = result.Where(e => e.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(sortByPrice))
            {
                if (sortByPrice.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(e => e.Price);
                }
                else if (sortByPrice.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(e => e.Price);
                }
            }
            result = result.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return Ok(result);
        }
    }
}
