
using Bingi_Storage.Models;

namespace Bingi_Storage.ViewModels
{
    public class ProductCreateViewModel
    {
        public struct ProductMedia
        {
            public string MediaType { get; set; } // e.g., "image", "video"
            public string Url { get; set; }
        }
        public struct ProductPayload
        {
            public string PayloadType { get; set; } // e.g., "file", "link"
            public IFormFile PayloadFile { get; set; }
            public string Url { get; set; } // Optional, for links
            public string TargetPlatform { get; set; } // e.g., "Windows", "Linux", "MacOS"
            public bool IsDemo { get; set; }
        }
        public struct PublisherInput
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Country { get; set; }
            public int? Rating { get; set; }
            public decimal? RevenueShare { get; set; }
            
        }
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
        public IFormFile? ImageFile { get; set; }
        public string? SystemRequirements { get; set; }
        public string? Documentation { get; set; }
        public string? Tags { get; set; }
        public string? Genre { get; set; }
        public string? ExternalLinks { get; set; }
        public int? AgeRestriction { get; set; }
        public ICollection<ProductMedia>? Media { get; set; } = new List<ProductMedia>();
        public ICollection<ProductPayload>? Payloads { get; set; } = new List<ProductPayload>();
        public PublisherInput? Publisher { get; set; }
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
