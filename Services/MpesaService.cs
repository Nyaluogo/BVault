using Bingi_Storage.Models;
using Bingi_Storage.ViewModels;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Bingi_Storage.Services
{
    public class MpesaService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _shortCode;
        private readonly string _passKey;
        private readonly string _callbackUrl;
        private readonly string _baseUrl = "https://sandbox.safaricom.co.ke";
        private readonly ILogger<MpesaService> _logger;

        public MpesaService(IConfiguration configuration, HttpClient httpClient, ILogger<MpesaService> logger)
        {
            _httpClient = httpClient;
            _consumerKey = configuration["Mpesa:ConsumerKey"];
            _consumerSecret = configuration["Mpesa:ConsumerSecret"];
            _shortCode = configuration["Mpesa:ShortCode"];
            _passKey = configuration["Mpesa:PassKey"];
            _callbackUrl = configuration["Mpesa:CallbackUrl"];
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _logger = logger;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_consumerKey}:{_consumerSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

            var response = await _httpClient.GetAsync("/oauth/v1/generate?grant_type=client_credentials");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonElement>();
                return content.GetProperty("access_token").GetString();
            }

            throw new Exception("Failed to get M-Pesa access token");
        }

        public async Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethodId, string returnUrl, string? phoneNum = null, string? email = null)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Format timestamp as yyyyMMddHHmmss
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                
                // Hardcode the sandbox shortcode and construct password
                string sandboxShortcode = "174379";
                string passwordString = $"{sandboxShortcode}{_passKey}{timestamp}";
                string password = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordString));

                // Calculate the actual amount (use at least 1 to avoid validation errors)
                int amount = Math.Max(1, (int)Math.Ceiling(order.TotalAmount ?? 1));

                // Validate phone number format (should be like 254XXXXXXXXX)
                if (string.IsNullOrEmpty(phoneNum))
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Phone number is required for M-Pesa payments."
                    };
                }

                // Ensure phone number is in the right format (254XXXXXXXXX)
                if (!phoneNum.StartsWith("254") && phoneNum.StartsWith("0"))
                {
                    phoneNum = "254" + phoneNum.Substring(1);
                }
                else if (!phoneNum.StartsWith("254"))
                {
                    phoneNum = "254" + phoneNum;
                }

                var stkRequest = new
                {
                    BusinessShortCode = sandboxShortcode,
                    Password = password,
                    Timestamp = timestamp,
                    TransactionType = "CustomerPayBillOnline",
                    Amount = amount,
                    PartyA = 254716900017,
                    PartyB = sandboxShortcode,
                    PhoneNumber = phoneNum,
                    CallBackURL = _callbackUrl,
                    AccountReference = $"Bingi-{order.Id}",
                    TransactionDesc = "Payment for games"
                };

                _logger.LogInformation($"STK Request: {JsonSerializer.Serialize(stkRequest)}");

                var response = await _httpClient.PostAsJsonAsync("/mpesa/stkpush/v1/processrequest", stkRequest);

                // Log raw response for debugging
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"M-Pesa Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<JsonElement>();

                    if (content.TryGetProperty("ResponseCode", out var responseCodeElement))
                    {
                        var responseCode = responseCodeElement.GetString();

                        if (responseCode == "0")
                        {
                            var checkoutRequestId = content.GetProperty("CheckoutRequestID").GetString();

                            return new PaymentResult
                            {
                                Success = true,
                                TransactionId = checkoutRequestId,
                                Message = "STK push sent. Please check your phone to complete payment."
                            };
                        }
                    }
                }

                return new PaymentResult
                {
                    Success = false,
                    Message = $"Failed to initiate M-Pesa payment: {responseBody}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing M-Pesa payment");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<PaymentResult> CheckPaymentStatusAsync(string checkoutRequestId)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Format timestamp as yyyyMMddHHmmss
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string sandboxShortcode = "174379";
                string password = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{sandboxShortcode}{_passKey}{timestamp}"));

                var queryRequest = new
                {
                    BusinessShortCode = sandboxShortcode,
                    Password = password,
                    Timestamp = timestamp,
                    CheckoutRequestID = checkoutRequestId
                };

                var response = await _httpClient.PostAsJsonAsync("/mpesa/stkpushquery/v1/query", queryRequest);

                // Log the response for debugging
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"M-Pesa Status Query Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<JsonElement>();
                    
                    // Check if the property exists before trying to access it
                    if (content.TryGetProperty("ResultCode", out var resultCodeElement))
                    {
                        var resultCode = resultCodeElement.GetString();
                        if (resultCode == "0")
                        {
                            return new PaymentResult
                            {
                                Success = true,
                                Message = "Payment completed successfully"
                            };
                        }
                        else
                        {
                            // Try to get the result description if available
                            string resultDesc = "Payment is still being processed";
                            if (content.TryGetProperty("ResultDesc", out var resultDescElement))
                            {
                                resultDesc = resultDescElement.GetString();
                            }

                            return new PaymentResult
                            {
                                Success = false,
                                Message = resultDesc
                            };
                        }
                    }

                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Payment is still being processed"
                    };
                }

                return new PaymentResult
                {
                    Success = false,
                    Message = $"Failed to check M-Pesa payment status: {responseBody}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking M-Pesa payment status");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Error checking status: {ex.Message}"
                };
            }
        }
    }
}
