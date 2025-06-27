using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingi_Storage.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }="Untitled Product";
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }=0.0m;
        public decimal? FileSize { get; set; }=0.0m; // Size in MB
        public decimal? Version { get; set; }
        public string? ImageUrl { get; set; }
        public string? SystemRequirements { get; set; }
        public int? AgeRestriction { get; set; }
        public int? DownloadCount { get; set; }
        public decimal? AverageRating { get; set; }
        public int? TotalRatings { get; set; }
        public ICollection<ProductMedia>? ProductMedia { get; set; }
        public Publisher? Publisher { get; set; }
        public ICollection<ProductCategory>? Category { get; set; }
        public bool IsBettingEnabled { get; set; }
        public enum PublishingStatus { DRAFT, PUBLISHED, SUSPENDED, DELETED}
        public PublishingStatus ProductPublishingStatus { get; set; } = PublishingStatus.DRAFT;
        public DateTime? ReleaseDate { get; set; }
        public DateTime? SuspensionDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
