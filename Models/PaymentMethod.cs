namespace Bingi_Storage.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public enum Type { CARD, BANK_TRANSFER, E_WALLET, CRYPTO }
        public Type? PayType { get; set; }
        public enum Provider { MASTERCARD, VISA, PAYPAL, STRIPE, MPESA, AMONEY }
        public Provider? TransProvider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
