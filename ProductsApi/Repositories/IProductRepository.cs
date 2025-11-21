using ProductsApi.Models;

namespace ProductsApi.Repositories
{
 
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id, bool includeDeleted = false);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<(IEnumerable<Product> Items, int Total)> ListAsync(string? keyword, string? category, int page = 1, int pageSize = 10, bool includeDeleted = false);
        Task<bool> NameExistsAsync(string name, Guid? excludeId = null);
    }

    
}
