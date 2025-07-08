using Bingi_Storage.Models;

namespace Bingi_Storage.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethodId, string returnUrl, string? phoneNum = null, string? email = null);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }
}
