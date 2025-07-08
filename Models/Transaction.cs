using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.Models
{
    public class Transaction
    {
        [Key]

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Order? TransactionOrder { get; set; }
        public AppUser User { get; set; }
        public Guid? WalletId { get; set; }
        public UserWallet? Wallet { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public enum Type { DEPOSIT, WITHDRAWAL, PURCHASE, BET, PAYOUT, REFUND}
        public Type PayType { get; set; }
        public enum Status { PENDING, COMPLETED, FAILED, CANCELLED, BLOCKED }
        public Status PayStatus { get; set; }
        // External reference for payment provider transaction IDs
        public string? ExternalReference { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
