namespace ProductsApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        Task<int> SaveChangesAsync();
        Task RollbackAsync();
    }
}

