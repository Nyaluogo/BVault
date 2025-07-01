namespace Bingi_Storage.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; } // e.g., "Product"
        public int EntityId { get; set; }
        public string Action { get; set; } // e.g., "Create", "Update", "Delete"
        public string PerformedBy { get; set; } // UserId
        public DateTime Timestamp { get; set; }
    }
}
