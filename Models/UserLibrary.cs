namespace Bingi_Storage.Models
{
    public class UserLibrary
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        // Foreign key for AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Many-to-many with Product
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public bool IsPublic { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
