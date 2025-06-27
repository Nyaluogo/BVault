namespace Bingi_Storage.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public enum PublishingStatus { DRAFT, PUBLISHED, SUSPENDED, DELETED }
        public PublishingStatus ReviewPublishingStatus { get; set; } = PublishingStatus.DRAFT;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
