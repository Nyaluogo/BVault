namespace Bingi_Storage.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Guid? TransactionId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public AppUser User { get; set; }
        public decimal? TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Transaction? OrderTransaction { get; set; }
        public enum Status { PENDING, COMPLETED, CANCELLED, REFUNDED}
        public Status status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
