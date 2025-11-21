using ProductsApi.Models;
using System.Collections.Concurrent;

namespace ProductsApi.Repositories
{
    public interface IDataStore
    {
        ConcurrentDictionary<Guid, Product> Products { get; }
    }
}

