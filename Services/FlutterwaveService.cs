using Bingi_Storage.Models;
using System.Text.Json;

namespace Bingi_Storage.Services
{
    public class FlutterwaveService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;
        private readonly string _publicKey;
        private readonly string _encryptionKey;
        private readonly string _baseUrl = "https://api.flutterwave.com/v3";

        public FlutterwaveService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _secretKey = configuration["Flutterwave:SecretKey"];
            _publicKey = configuration["Flutterwave:PublicKey"];
            _encryptionKey = configuration["Flutterwave:EncryptionKey"];
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_secretKey}");
        }

        public async Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethodId, string returnUrl, string? phoneNum = null, string? email = null)
        {
            try
            {
                var request = new
                {
                    tx_ref = $"bingi-{order.Id}-{DateTime.UtcNow.Ticks}",
                    amount = order.TotalAmount,
                    currency = "KES",
                    redirect_url = returnUrl,
                    payment_options = "card,account,ussd,mpesa,barter,credit",
                    customer = new
                    {
                        email = order.User.Email,
                        name = $"{order.User.FirstName} {order.User.OtherName}",
                        phone_number = order.User.PhoneNumber
                    },
                    customizations = new
                    {
                        title = "Bingi Storage Payment",
                        description = order.Description,
                        logo = "https://your-logo-url.com/logo.png"
                    }
                };

                var response = await _httpClient.PostAsJsonAsync("/payments", request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<JsonElement>();
                    var status = content.GetProperty("status").GetString();

                    if (status == "success")
                    {
                        var data = content.GetProperty("data");
                        var link = data.GetProperty("link").GetString();

                        return new PaymentResult
                        {
                            Success = true,
                            RedirectUrl = link,
                            Message = "Redirect to Flutterwave payment page"
                        };
                    }
                }

                return new PaymentResult
                {
                    Success = false,
                    Message = "Failed to initialize Flutterwave payment"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
