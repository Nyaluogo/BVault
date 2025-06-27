namespace Bingi_Storage.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
