using Microsoft.AspNetCore.Mvc;
using BE.Models;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product> { };

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = products.FirstOrDefault(e => e.Id == id);
            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm với ID: {id}" });
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            if (products.Any())
            {
                product.Id = products.Max(e => e.Id) + 1;
            }
            else
            {
                product.Id = 1;
            }
            products.Add(product);
            return Ok(new { message = "Đã thêm thông tin sản phẩm thành công." });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = products.FirstOrDefault(e => e.Id == id);
            if (product == null)
            {
                return NotFound(new { message = $"Không tìm thấy sản phẩm với ID: {id}" });
            }
            //product.Name = updatedProduct.Name;
            //product.Position = updatedProduct.Position;
            //product.Salary = updatedProduct.Salary;
            return Ok(new { message = $"Thông tin sản phẩm với id: {id} đã được cập nhật." });
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(e => e.Id == id);
            if (product == null) return NotFound();
            products.Remove(product);
            return Ok(new { message = $"Đã xóa sản phẩm với ID: {id} thành công" });
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Product>> SearchProducts(
            [FromQuery] string? name,
            [FromQuery] string? position,
            [FromQuery] string? sortBySalary)
        {
            var result = products.AsEnumerable();
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(position))
            {
                result = result.Where(e => e.Position.Contains(position, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(sortBySalary))
            {
                if (sortBySalary.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(e => e.Salary);
                }
                else if (sortBySalary.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(e => e.Salary);
                }
            }
            return Ok(result);
        }

    }
}
