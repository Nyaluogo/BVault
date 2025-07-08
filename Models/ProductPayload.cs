using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingi_Storage.Models
{
    public class ProductPayload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // Foreign key
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Title { get; set; } = "Untitled Product";
        public string? ShortDescription { get; set; }
        public string? FileSize { get; set; }

        [Required]
        public string FileUrl { get; set; }= string.Empty;
        public decimal? CustomPrice { get; set; } = 0.0m;
        public int DownloadCount { get; set; } = 0;
        public enum PublishingStatus { DRAFT, PUBLISHED, SUSPENDED, DELETED }
        public PublishingStatus ProductPublishingStatus { get; set; } = PublishingStatus.DRAFT;
        public enum Type { BIN_WIN, BIN_LINUX, BIN_ANDROID, BIN_WEB, UNDEFINED }
        public Type _Type { get; set; } = ProductPayload.Type.UNDEFINED;
        public bool IsDemo { get; set; } = false;
        
        // Unity WebGL specific properties
        public string? UnityVersion { get; set; }
        public string? BuildTarget { get; set; } = "WebGL";
        public bool IsUnityWebGL => _Type == Type.BIN_WEB;

        // Game configuration
        public string? GameDataUrl { get; set; }
        public string? GameFrameworkUrl { get; set; }
        public string? GameCodeUrl { get; set; }
        public string? GameSymbolsUrl { get; set; }

        // Recommended canvas size for Unity WebGL
        public int? CanvasWidth { get; set; } = 960;
        public int? CanvasHeight { get; set; } = 600;

        // Game metadata
        public string? GameControls { get; set; } // JSON string for control mappings
        public bool RequiresKeyboard { get; set; } = true;
        public bool RequiresMouse { get; set; } = true;
        public bool SupportsMobile { get; set; } = false;
        public bool SupportsFullscreen { get; set; } = true;

        // Performance settings
        public string? QualitySettings { get; set; } // Low, Medium, High
        public bool EnableAudioAutoPlay { get; set; } = true;
        public bool EnableWebGLStreaming { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
