using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingi_Storage.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string? CustomUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal? DefaultPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? FileSize { get; set; } // Size in MB
        public decimal? Version { get; set; }
        public string? VideoTrailerUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? SystemRequirements { get; set; }
        public string? Documentation { get; set; }
        public string? Tags { get; set; }
        public string? Genre { get; set; }
        public string? ExternalLinks { get; set; }
        public int? AgeRestriction { get; set; }
        public int? DownloadCount { get; set; }
        public decimal? AverageRating { get; set; }
        public int? TotalRatings { get; set; }
        public ICollection<Bundle>? Bundles { get; set; }
        public ICollection<ProductPrice>? Prices { get; set; }
        public ICollection<Platform>? Platforms { get; set; }
        public ICollection<ProductMedia>? Media { get; set; }
        public ICollection<ProductPayload>? Payloads { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<ProductCategory>? Category { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public bool IsBettingEnabled { get; set; }
        public bool IsAIGen { get; set; } = false;
        public enum PricingStatus { PAID, FREE, DONATION }
        public PricingStatus PricingState { get; set; } = PricingStatus.DONATION;
        public enum PublishingStatus { DRAFT, PUBLISHED, SUSPENDED, DELETED }
        public PublishingStatus ProductPublishingStatus { get; set; } = PublishingStatus.DRAFT;
        public int? ParentProductId { get; set; }
        public Product? ParentProduct { get; set; }
        public ICollection<Product>? ChildProducts { get; set; } // For DLCs or bundles
        public DateTime? ReleaseDate { get; set; }
        public DateTime? SuspensionDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
