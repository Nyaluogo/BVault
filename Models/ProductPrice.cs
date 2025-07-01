namespace Bingi_Storage.Models
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Region { get; set; } // e.g., "US", "EU", "JP"
        public decimal Price { get; set; }
    }
}
