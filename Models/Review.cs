using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int Rating { get; set; } // 1-5 stars
        public string Comment { get; set; } = string.Empty;
        public enum PublishingStatus { DRAFT, PUBLISHED, SUSPENDED, DELETED }
        public PublishingStatus ReviewPublishingStatus { get; set; } = PublishingStatus.DRAFT;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
