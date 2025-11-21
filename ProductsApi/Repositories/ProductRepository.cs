using ProductsApi.Models;
using System.Collections.Concurrent;

namespace ProductsApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDataStore _dataStore;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public ProductRepository(IDataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        private ConcurrentDictionary<Guid, Product> Store => _dataStore.Products;

        public Task<Product> CreateAsync(Product product)
        {
            Store[product.Id] = product;
            return Task.FromResult(product);
        }

        public Task<Product?> GetByIdAsync(Guid id, bool includeDeleted = false)
        {
            if (Store.TryGetValue(id, out var p))
                return Task.FromResult(includeDeleted ? p : (p.IsDeleted ? null : p));
            return Task.FromResult<Product?>(null);
        }

        public Task<Product?> UpdateAsync(Product product)
        {
            if (!Store.ContainsKey(product.Id)) return Task.FromResult<Product?>(null);
            Store[product.Id] = product;
            return Task.FromResult<Product?>(product);
        }

        public Task<bool> SoftDeleteAsync(Guid id)
        {
            if (!Store.TryGetValue(id, out var p)) return Task.FromResult(false);
            p.IsDeleted = true;
            Store[id] = p;
            return Task.FromResult(true);
        }

        public Task<(IEnumerable<Product> Items, int Total)> ListAsync(string? keyword, string? category, int page = 1, int pageSize = 10, bool includeDeleted = false)
        {
            var q = Store.Values.AsEnumerable();
            if (!includeDeleted) q = q.Where(x => !x.IsDeleted);
            if (!string.IsNullOrWhiteSpace(keyword)) q = q.Where(x => x.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(category)) q = q.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            var total = q.Count();
            var items = q.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult((items.AsEnumerable(), total));
        }

        public async Task<bool> NameExistsAsync(string name, Guid? excludeId = null)
        {
            // Use lock to avoid race condition between check and create/update
            await _lock.WaitAsync();
            try
            {
                return Store.Values.Any(p => !p.IsDeleted &&
                    p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    (!excludeId.HasValue || p.Id != excludeId.Value));
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}

