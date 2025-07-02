using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string? SessionId { get; set; } // For non-logged in users
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Cart summary properties
        public decimal Subtotal => Items.Sum(i => i.Quantity * i.UnitPrice);
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total => Subtotal - Discount + Tax;
    }

    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public ShoppingCart Cart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public string? SelectedOptions { get; set; } // JSON string for additional options
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
