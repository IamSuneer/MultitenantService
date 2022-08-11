using MultitenantService.Entities;

namespace MultitenantService.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(string name, string description, decimal price);
        Task<Product> GetByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
