using Microsoft.EntityFrameworkCore;
using MultitenantService.Data;
using MultitenantService.Entities;

namespace MultitenantService.Services
{
    public class ProductService : IProductService
    {
        private readonly MultitenantDbContext _context;

        public ProductService(MultitenantDbContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateAsync(string name, string description, decimal price)
        {
            var product = new Product(name, description, price);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
