namespace Bingi_Storage.Models
{
    public class UserWallet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public enum Status { ACTIVE, FROZEN, CLOSED }
        public Status WalletStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
