using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BE.Data;
using BE.Models;

namespace BE.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddProductAsync(ProductModel model)
        {
            var newProduct = _mapper.Map<Product>(model);
            _context.Products!.Add(newProduct);
            await _context.SaveChangesAsync();

            return newProduct.Id;
        }

        public async Task DeleteProductAsync(int id)
        {
            var deleteProduct = _context.Products!.SingleOrDefault(b => b.Id == id);
            if (deleteProduct != null)
            {
                _context.Products!.Remove(deleteProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var Products = await _context.Products!.ToListAsync();
            return _mapper.Map<List<ProductModel>>(Products);
        }

        public async Task<ProductModel> GetProductAsync(int id)
        {
            var Product = await _context.Products!.FindAsync(id);
            return _mapper.Map<ProductModel>(Product);
        }

        public async Task UpdateProductAsync(int id, ProductModel model)
        {
            if (id == model.Id)
            {
                var updateProduct = _mapper.Map<Product>(model);
                _context.Products!.Update(updateProduct);
                await _context.SaveChangesAsync();
            }
        }
    }
}