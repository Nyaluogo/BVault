namespace Bingi_Storage.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public enum Type { CARD, BANK_TRANSFER, E_WALLET, CRYPTO, MOBILE_MONEY }
        public Type? PayType { get; set; }
        public enum Provider { MASTERCARD, VISA, PAYPAL, STRIPE, MPESA, AMONEY, FLUTTERWAVE, AIRTEL_MONEY, T_KASH, CRYPTO_WALLET }
        public Provider? TransProvider { get; set; }
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
