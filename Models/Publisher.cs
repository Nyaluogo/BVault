using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.Models
{
    public class Publisher
    {
        //public Publisher(string appUserId, string name)
        //{
        //    AppUserId = appUserId ?? throw new ArgumentNullException(nameof(appUserId));
        //    Name = name ?? throw new ArgumentNullException(nameof(name));
        //    CreatedAt = DateTime.UtcNow;
        //    UpdatedAt = DateTime.UtcNow;
        //}
        //public Publisher() { } // Parameterless constructor for EF Core
        [Key]
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
        // Relationship with AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Relationship with Products
        public ICollection<Product>? Products { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
