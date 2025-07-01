using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.Models
{
    public class ProductMedia
    {
        [Key]
        public Guid Id { get; set; }
        // Foreign key
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? PreviewImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int DisplayOrder { get; set; }
        public enum Type { LOGO, HEADER, SCREENSHOT, TRAILER, BANNER, POSTER, THUMBNAIL }
        public Type MediaType { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
