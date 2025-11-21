namespace ProductsApi.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public Decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
