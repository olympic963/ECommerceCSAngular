using BE.Data;
using BE.Models;

namespace BE.Repositories
{
    public interface IProductRepository
    {
        public Task<List<ProductModel>> GetAllProductsAsync();
        public Task<ProductModel> GetProductAsync(int id);
        public Task<int> AddProductAsync(ProductModel model);
        public Task UpdateProductAsync(int id, ProductModel model);
        public Task DeleteProductAsync(int id);
    }
}