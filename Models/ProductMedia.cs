namespace Bingi_Storage.Models
{
    public class ProductMedia
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? BinaryUrl { get; set; }
        public int DisplayOrder { get; set; }
        public enum Type { LOGO, HEADER, SCREENSHOT, TRAILER, BANNER, POSTER, THUMBNAIL }
        public Type MediaType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
