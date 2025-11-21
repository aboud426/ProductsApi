namespace ProductsApi.DTOs
{
    public class PaginatedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
public class PaginatedRequestParameters
{
    public int page;
    public int pageSize;
}