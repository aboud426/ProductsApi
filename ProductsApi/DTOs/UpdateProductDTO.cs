using System.ComponentModel.DataAnnotations;

namespace ProductsApi.DTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }

        
    }
}
