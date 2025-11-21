using FluentValidation;
using ProductsApi.DTOs;
using ProductsApi.Exceptions;
using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateProductDTO> _createValidator;
        private readonly IValidator<UpdateProductDTO> _updateValidator;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IUnitOfWork unitOfWork,
            IValidator<CreateProductDTO> createValidator,
            IValidator<UpdateProductDTO> updateValidator,
            ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            if (await _unitOfWork.Products.NameExistsAsync(dto.Name))
            {
                throw new InvalidOperationException("Name must be unique.");
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.Products.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(product);
        }

        public async Task<ProductDTO> GetByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, includeDeleted: false);
            if (product == null)
            {
                throw new NotFoundException("Product", id);
            }
            return MapToDto(product);
        }

        public async Task<ProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            var existing = await _unitOfWork.Products.GetByIdAsync(id, includeDeleted: false);
            if (existing == null)
            {
                throw new NotFoundException("Product", id);
            }

            if (await _unitOfWork.Products.NameExistsAsync(dto.Name, excludeId: id))
            {
                throw new InvalidOperationException("Name must be unique.");
            }

            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.Price = dto.Price;

            var updated = await _unitOfWork.Products.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
            
            if (updated == null)
            {
                throw new NotFoundException("Product", id);
            }
            
            return MapToDto(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await _unitOfWork.Products.SoftDeleteAsync(id);
            if (!result)
            {
                throw new NotFoundException("Product", id);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PaginatedResult<ProductDTO>> ListAsync(string? keyword, string? category, int page, int pageSize, bool includeDeleted)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var (items, total) = await _unitOfWork.Products.ListAsync(keyword, category, page, pageSize, includeDeleted);

            return new PaginatedResult<ProductDTO>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items.Select(MapToDto)
            };
        }

        private static ProductDTO MapToDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            IsDeleted = product.IsDeleted
        };
    }
}

