namespace Bingi_Storage.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Windows", "MacOS", "Linux"
        public ICollection<Product> Products { get; set; } = [];
    }
}
