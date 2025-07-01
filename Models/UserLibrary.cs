namespace Bingi_Storage.Models
{
    public class UserLibrary
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsPublic { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
