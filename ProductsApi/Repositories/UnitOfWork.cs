using ProductsApi.Models;
using System.Collections.Concurrent;

namespace ProductsApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataStore _dataStore;
        private IProductRepository? _productRepository;
        private bool _disposed = false;

        public UnitOfWork(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public IProductRepository Products
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(UnitOfWork));

                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_dataStore);
                }
                return _productRepository;
            }
        }

        public Task<int> SaveChangesAsync()
        {
            // For in-memory implementation, changes are already applied
            // In a real database implementation, this would save changes to the database
            return Task.FromResult(1);
        }

        public Task RollbackAsync()
        {
            // For in-memory implementation, we can't easily rollback
            // In a real database implementation, this would rollback the transaction
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources if needed
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

