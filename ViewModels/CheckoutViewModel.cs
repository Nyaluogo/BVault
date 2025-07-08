using Bingi_Storage.Models;
using System.ComponentModel.DataAnnotations;

namespace Bingi_Storage.ViewModels
{
    public class CheckoutViewModel
    {
        public ShoppingCart Cart { get; set; }

        public List<UserWallet> Wallets { get; set; }

        [Required(ErrorMessage = "Please select a wallet")]
        public Guid SelectedWalletId { get; set; }

        public List<PaymentMethod> AvailablePaymentMethods { get; set; }

        // Additional billing information if needed
        public string BillingName { get; set; }
        public string BillingEmail { get; set; }
        public string BillingCountry { get; set; }
        [Required(ErrorMessage = "Please enter phone number")]
        public string MpesaPhoneNumber { get; set; }
    }
}
