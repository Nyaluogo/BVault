namespace Bingi_Storage.Models
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string? Bio { get; set; }
        public string? Country { get; set; }
        public AppUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
