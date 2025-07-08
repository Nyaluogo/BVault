using Bingi_Storage.Models;
using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.ViewModels
{
    public class ProductCreateViewModel
    {
        public AppUser? _AppUser { get; set; }
        public Publisher? _Publisher { get; set; }

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

            // Unity WebGL specific properties
            public string? UnityVersion { get; set; }
            public int? CanvasWidth { get; set; }
            public int? CanvasHeight { get; set; }
            public bool RequiresKeyboard { get; set; }
            public bool RequiresMouse { get; set; }
            public bool SupportsMobile { get; set; }
            public string? GameControls { get; set; }
        }

        public class PublisherInput
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Publisher name is required")]
            [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters")]
            public string Name { get; set; } = string.Empty;

            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
            public string? Description { get; set; }

            [EmailAddress(ErrorMessage = "Please enter a valid email address")]
            public string? Email { get; set; }

            [Phone(ErrorMessage = "Please enter a valid phone number")]
            public string? Phone { get; set; }

            [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters")]
            public string? Country { get; set; }

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
            public int? Rating { get; set; }

            [Range(0, 100, ErrorMessage = "Revenue share must be between 0 and 100")]
            public decimal? RevenueShare { get; set; }
        }

        [Required(ErrorMessage = "Product title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = "Untitled Product";

        [StringLength(100, ErrorMessage = "Custom URL cannot exceed 100 characters")]
        public string? CustomUrl { get; set; }

        [StringLength(200, ErrorMessage = "Short description cannot exceed 200 characters")]
        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal? DefaultPrice { get; set; } = 0.0m;

        [Range(0, double.MaxValue, ErrorMessage = "Sale price must be a positive number")]
        public decimal? SalePrice { get; set; } = 0.0m;

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public decimal? Discount { get; set; } = 0.0m;

        [Range(0, double.MaxValue, ErrorMessage = "Version must be a positive number")]
        public decimal? Version { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? VideoTrailerUrl { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? SystemRequirements { get; set; }
        public string? Documentation { get; set; }
        public string? Tags { get; set; }
        public string? Genre { get; set; }
        public string? ExternalLinks { get; set; }

        [Range(0, 21, ErrorMessage = "Age restriction must be between 0 and 21")]
        public int? AgeRestriction { get; set; }

        // Unity WebGL specific properties
        public bool IsUnityGame { get; set; } = false;
        public string? UnityVersion { get; set; }
        public int CanvasWidth { get; set; } = 960;
        public int CanvasHeight { get; set; } = 600;
        public bool RequiresKeyboard { get; set; } = true;
        public bool RequiresMouse { get; set; } = true;
        public bool SupportsMobile { get; set; } = false;
        public string? GameControls { get; set; }


        public ICollection<ProductMedia>? Media { get; set; } = new List<ProductMedia>();
        public ICollection<ProductPayload>? Payloads { get; set; } = new List<ProductPayload>();

        public PublisherInput Publisher { get; set; } = new PublisherInput();

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