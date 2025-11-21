using ProductsApi.DTOs;

namespace ProductsApi.Services
{
    public interface IProductService
    {
        Task<ProductDTO> CreateAsync(CreateProductDTO dto);
        Task<ProductDTO> GetByIdAsync(Guid id);
        Task<ProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto);
        Task DeleteAsync(Guid id);
        Task<PaginatedResult<ProductDTO>> ListAsync(string? keyword, string? category, int page, int pageSize, bool includeDeleted);
    }
}

