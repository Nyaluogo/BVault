namespace Bingi_Storage.Models
{
    public class ProductMetrics
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Views { get; set; }
        public int Purchases { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }
}
