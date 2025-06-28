
using Bingi_Storage.Models;

namespace Bingi_Storage.ViewModels
{
    public class ProductCreateViewModel
    {
        public string Title { get; set; } = "Untitled Product";
        public string? CustomUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal? DefaultPrice { get; set; } = 0.0m;
        public decimal? SalePrice { get; set; } = 0.0m;
        public decimal? Discount { get; set; } = 0.0m;
        public decimal? Version { get; set; }
        public string? VideoTrailerUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? SystemRequirements { get; set; }
        public string? Documentation { get; set; }
        public string? Tags { get; set; }
        public string? Genre { get; set; }
        public string? ExternalLinks { get; set; }
        public int? AgeRestriction { get; set; }
        public ICollection<ProductMedia>? Media { get; set; } = new List<ProductMedia>();
        public ICollection<ProductPayload>? Payloads { get; set; } = new List<ProductPayload>();
        public Publisher? Publisher { get; set; }
        public ICollection<ProductCategory>? Category { get; set; }
        public bool IsBettingEnabled { get; set; }
        public bool IsAIGen { get; set; } = false;
        public enum PricingStatus { PAID, FREE, DONATION }
        public PricingStatus PricingState { get; set; } = PricingStatus.DONATION;
        public enum PublishingStatus { DRAFT, PUBLISHED, SUSPENDED, DELETED }
        public PublishingStatus ProductPublishingStatus { get; set; } = PublishingStatus.DRAFT;
        public DateTime? ReleaseDate { get; set; }
    }
}
