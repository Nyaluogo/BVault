namespace Bingi_Storage.Models
{
    public class Bundle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
