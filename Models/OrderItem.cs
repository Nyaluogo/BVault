using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
