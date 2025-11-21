using ProductsApi.Models;
using System.Collections.Concurrent;

namespace ProductsApi.Repositories
{
    public class InMemoryDataStore : IDataStore
    {
        public ConcurrentDictionary<Guid, Product> Products { get; } = new();
    }
}

