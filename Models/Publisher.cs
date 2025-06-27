namespace Bingi_Storage.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public int? Rating { get; set; }
        public decimal? RevenueShare { get; set; }
        public enum Status { PENDING, VERIFIED, FEATURED, CANCELLED, BLOCKED }
        public Status? PublicityStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
