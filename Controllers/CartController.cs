using Bingi_Storage.Data;
using Bingi_Storage.Models;
using Bingi_Storage.Services;
using Bingi_Storage.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bingi_Storage.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CartController> _logger;


        public CartController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider,
            IWebHostEnvironment hostEnv,
            ILogger<CartController> logger)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _webHostEnvironment = hostEnv;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string returnUrl)
        {
            // Find the product
            var product = await _context.Product
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return NotFound();

            // Get or create cart
            var cart = await GetOrCreateCartAsync();

            // Check if product is already in cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Game already in cart - show notification
                TempData["CartMessage"] = "This game is already in your cart.";
            }
            else
            {
                // Add new cart item
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    UnitPrice = product.SalePrice ?? product.DefaultPrice ?? 0,
                    Quantity = 1,
                    AddedAt = DateTime.UtcNow
                };

                cart.Items.Add(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                TempData["CartMessage"] = $"{product.Title} added to your cart.";
            }

            // Return to the page they came from
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            var cart = await GetOrCreateCartAsync();

            // Load related product data
            var cartWithProducts = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            return View(cartWithProducts);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var cart = await GetOrCreateCartAsync();
            var item = await _context.CartItems.FindAsync(itemId);

            if (item != null && item.CartId == cart.Id)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var cart = await GetOrCreateCartAsync();
            var items = await _context.CartItems.Where(i => i.CartId == cart.Id).ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await GetOrCreateCartAsync();

            // Get full cart with products
            var cartWithProducts = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            if (cartWithProducts?.Items.Count == 0)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            // Get user wallet for payment
            var user = await _userManager.GetUserAsync(User);
            var wallets = await _context.UserWallets
                .Where(w => w.UserId == user.Id)
                .ToListAsync();

            var viewModel = new CheckoutViewModel
            {
                Cart = cartWithProducts,
                Wallets = wallets,
                AvailablePaymentMethods = await _context.PaymentMethods.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteCheckout(CheckoutViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.IsActive);

            if (!ModelState.IsValid)
            {
                model.Cart = cart;
                model.Wallets = await _context.UserWallets.Where(w => w.UserId == user.Id).ToListAsync();
                model.AvailablePaymentMethods = await _context.PaymentMethods.ToListAsync();
                return View("Checkout", model);
            }

            // Get the selected wallet
            var wallet = await _context.UserWallets.FindAsync(model.SelectedWalletId);

            if (wallet == null || wallet.UserId != user.Id)
            {
                ModelState.AddModelError("", "Invalid wallet selected.");
                model.Cart = cart;
                model.Wallets = await _context.UserWallets.Where(w => w.UserId == user.Id).ToListAsync();
                model.AvailablePaymentMethods = await _context.PaymentMethods.ToListAsync();
                return View("Checkout", model);
            }

            // Check if sufficient funds
            decimal totalAmount = cart.Total;

            if (wallet.Balance < totalAmount)
            {
                ModelState.AddModelError("", "Insufficient funds in the selected wallet.");
                model.Cart = cart;
                model.Wallets = await _context.UserWallets.Where(w => w.UserId == user.Id).ToListAsync();
                model.AvailablePaymentMethods = await _context.PaymentMethods.ToListAsync();
                return View("Checkout", model);
            }

            // Create transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Name = "Game Purchase",
                Description = $"Purchase of {cart.Items.Count} item(s)",
                User = user,
                WalletId = wallet.Id,
                Wallet = wallet,
                Amount = totalAmount,
                PayType = Transaction.Type.PURCHASE,
                PayStatus = Transaction.Status.PENDING,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create order
            var order = new Order
            {
                Name = "Game Purchase",
                Description = $"Purchase of {cart.Items.Count} item(s)",
                User = user,
                TotalAmount = totalAmount,
                status = Order.Status.PROCESSING,
                TransactionId = transaction.Id,
                OrderTransaction = transaction,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            transaction.TransactionOrder = order;

            // Create order items
            var orderItems = cart.Items.Select(item => new OrderItem
            {
                Name = item.Product.Title,
                Description = item.Product.ShortDescription ?? "",
                UnitPrice = item.UnitPrice,
                TotalPrice = item.UnitPrice * item.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            // Update wallet balance
            wallet.Balance -= totalAmount;
            wallet.UpdatedAt = DateTime.UtcNow;

            // Update transaction and order status
            transaction.PayStatus = Transaction.Status.COMPLETED;
            order.status = Order.Status.COMPLETED;
            order.CompletedAt = DateTime.UtcNow;

            // Save everything to the database
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Transactions.Add(transaction);
                    _context.Orders.Add(order);
                    _context.OrderItems.AddRange(orderItems);
                    _context.Update(wallet);

                    // Clear cart
                    var cartItems = await _context.CartItems.Where(i => i.CartId == cart.Id).ToListAsync();
                    _context.CartItems.RemoveRange(cartItems);

                    await _context.SaveChangesAsync();
                    dbTransaction.Commit();

                    // Add games to user library
                    var library = await _context.UserLibraries.FirstOrDefaultAsync(l => l.AppUserId == user.Id);

                    if (library == null)
                    {
                        library = new UserLibrary
                        {
                            AppUserId = user.Id,
                            Name = "My Games",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        _context.UserLibraries.Add(library);
                        await _context.SaveChangesAsync();
                    }

                    // Add purchased products to library
                    foreach (var item in cart.Items)
                    {
                        var product = await _context.Product.FindAsync(item.ProductId);
                        library.Products.Add(product);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    ModelState.AddModelError("", $"Error processing order: {ex.Message}");
                    model.Cart = cart;
                    model.Wallets = await _context.UserWallets.Where(w => w.UserId == user.Id).ToListAsync();
                    model.AvailablePaymentMethods = await _context.PaymentMethods.ToListAsync();
                    return View("Checkout", model);
                }
            }
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.OrderTransaction)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // Helper method to get or create a cart for the current user/session
        private async Task<ShoppingCart> GetOrCreateCartAsync()
        {
            ShoppingCart cart = null;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                // Try to find an active cart for this user
                cart = await _context.ShoppingCarts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id && c.IsActive);

                if (cart == null)
                {
                    // Create a new cart
                    cart = new ShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        User = user,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.ShoppingCarts.Add(cart);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                // Handle anonymous shopping
                string sessionId = _httpContextAccessor.HttpContext.Session.Id;

                // Try to find a cart for this session
                cart = await _context.ShoppingCarts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.IsActive);

                if (cart == null)
                {
                    // Create a new cart
                    cart = new ShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        SessionId = sessionId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.ShoppingCarts.Add(cart);
                    await _context.SaveChangesAsync();
                }
            }

            return cart;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var cart = await GetOrCreateCartAsync();
            return Json(new { count = cart.Items.Count });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessExternalPayment(int paymentMethodId, Guid cartId, string returnUrl, string mpesaPhoneNumber)
        {
            try
            {
                var paymentMethod = await _context.PaymentMethods.FindAsync(paymentMethodId);
                if (paymentMethod == null)
                {
                    TempData["Error"] = "Payment method not found";
                    return RedirectToAction("Checkout");
                }

                // Get the cart with all related data
                var cart = await _context.ShoppingCarts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.Id == cartId);

                if (cart == null || !cart.Items.Any())
                {
                    TempData["Error"] = "Cart not found or empty";
                    return RedirectToAction("Checkout");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["Error"] = "User not authenticated";
                    return Challenge();
                }

                // Create a pending transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    Name = "External Payment",
                    Description = $"Purchase of {cart.Items.Count} item(s)",
                    User = user,
                    Amount = cart.Total,
                    PayType = Transaction.Type.PURCHASE,
                    PayStatus = Transaction.Status.PENDING,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PaymentMethodId = paymentMethodId
                };

                // Create a pending order
                var order = new Order
                {
                    Name = "Game Purchase",
                    Description = $"Purchase of {cart.Items.Count} item(s) via {paymentMethod.Name}",
                    User = user,
                    TotalAmount = cart.Total,
                    status = Order.Status.PROCESSING,
                    TransactionId = transaction.Id,
                    OrderTransaction = transaction,
                    PaymentMethodId = paymentMethodId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                transaction.TransactionOrder = order;

                // Create order items
                var orderItems = cart.Items.Select(item => new OrderItem
                {
                    Name = item.Product.Title,
                    Description = item.Product.ShortDescription ?? "",
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.UnitPrice * item.Quantity,
                    Order = order,
                    ProductId = item.ProductId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                order.Items = orderItems;

                // Save the transaction and order
                using (var dbTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Transactions.Add(transaction);
                        _context.Orders.Add(order);
                        _context.OrderItems.AddRange(orderItems);
                        await _context.SaveChangesAsync();
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                        // In development, show detailed error
                        if (_webHostEnvironment.IsDevelopment())
                        {
                            TempData["Error"] = $"Failed to create order: {innerMessage}";
                        }
                        else
                        {
                            // In production, log the error but show a generic message
                            _logger.LogError(ex, "Order creation failed: {Message}", innerMessage);
                            TempData["Error"] = "Failed to process your payment. Please try again or contact support.";
                        }

                        return RedirectToAction("Checkout");
                    }
                }

                // Debug info
                TempData["Debug"] = $"Processing payment: Method={paymentMethod.Name}, Provider={paymentMethod.TransProvider}, CartID={cartId}";

                // Process payment logic
                IPaymentService paymentService;
                try
                {
                    if (paymentMethod.TransProvider == PaymentMethod.Provider.FLUTTERWAVE)
                    {
                        paymentService = _serviceProvider.GetRequiredService<FlutterwaveService>();
                    }
                    else if (paymentMethod.TransProvider == PaymentMethod.Provider.MPESA)
                    {
                        paymentService = _serviceProvider.GetRequiredService<MpesaService>();
                    }
                    else
                    {
                        TempData["Error"] = $"Unsupported payment method: {paymentMethod.TransProvider}";
                        return RedirectToAction("Checkout");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Failed to initialize payment service: {ex.Message}";
                    return RedirectToAction("Checkout");
                }

                // Process the payment
                try
                {
                    var result = await paymentService.ProcessPaymentAsync(order, paymentMethodId.ToString(), returnUrl, mpesaPhoneNumber);

                    if (result.Success)
                    {
                        // For M-Pesa, store the checkout request ID as the external reference
                        if (paymentMethod.TransProvider == PaymentMethod.Provider.MPESA &&
                            !string.IsNullOrEmpty(result.TransactionId))
                        {
                            transaction.ExternalReference = result.TransactionId;
                            _context.Update(transaction);
                            await _context.SaveChangesAsync();
                        }

                        if (!string.IsNullOrEmpty(result.RedirectUrl))
                        {
                            return Redirect(result.RedirectUrl);
                        }

                        TempData["PaymentMessage"] = result.Message;
                        TempData["TransactionId"] = result.TransactionId;
                        return RedirectToAction("PaymentPending", new { orderId = order.Id });
                    }
                    else
                    {
                        TempData["Error"] = result.Message ?? "Payment processing failed with no specific error message";
                        return RedirectToAction("Checkout");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Payment processing error: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        TempData["Error"] += $" - {ex.InnerException.Message}";
                    }
                    return RedirectToAction("Checkout");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Unexpected error: {e.Message}";
                return RedirectToAction("Checkout");
            }

        }

        public async Task<IActionResult> PaymentCallback(string tx_ref, string transaction_id, string status)
        {
            // Handle callbacks from payment providers
            if (status.ToLower() == "successful")
            {
                // Extract order ID from tx_ref (format: bingi-{orderId}-{timestamp})
                var parts = tx_ref.Split('-');
                if (parts.Length > 1 && int.TryParse(parts[1], out int orderId))
                {
                    var order = await _context.Orders.FindAsync(orderId);
                    if (order != null)
                    {
                        order.status = Order.Status.COMPLETED;
                        order.CompletedAt = DateTime.UtcNow;

                        // Find transaction and update it
                        var transaction = await _context.Transactions.FindAsync(order.TransactionId);
                        if (transaction != null)
                        {
                            transaction.PayStatus = Transaction.Status.COMPLETED;
                            transaction.UpdatedAt = DateTime.UtcNow;
                            _context.Update(transaction);
                        }

                        _context.Update(order);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
                    }
                }
            }

            TempData["Error"] = "Payment was not successful. Please try again.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CheckPaymentStatus(int orderId, string transactionId)
        {
            try
            {
                // Get the order with payment details
                var order = await _context.Orders
                    .Include(o => o.OrderTransaction)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return Json(new { completed = false, message = "Order not found" });

                // If order is already completed, return success
                if (order.status == Order.Status.COMPLETED)
                    return Json(new { completed = true });

                // Get the payment method to determine which service to use
                var paymentMethod = await _context.PaymentMethods.FindAsync(order.PaymentMethodId);
                if (paymentMethod == null)
                    return Json(new { completed = false, message = "Payment method not found" });

                // For M-Pesa, check the status using the M-Pesa service
                if (paymentMethod.TransProvider == PaymentMethod.Provider.MPESA)
                {
                    var mpesaService = _serviceProvider.GetRequiredService<MpesaService>();
                    var status = await mpesaService.CheckPaymentStatusAsync(transactionId);

                    if (status.Success)
                    {
                        // Update order and transaction to completed
                        order.status = Order.Status.COMPLETED;
                        order.CompletedAt = DateTime.UtcNow;

                        var transaction = order.OrderTransaction;
                        if (transaction != null)
                        {
                            transaction.PayStatus = Transaction.Status.COMPLETED;
                            transaction.UpdatedAt = DateTime.UtcNow;
                            _context.Update(transaction);
                        }

                        _context.Update(order);
                        await _context.SaveChangesAsync();

                        // Add products to user library
                        await AddProductsToUserLibraryAsync(order);

                        return Json(new { completed = true });
                    }

                    return Json(new { completed = false, message = status.Message });
                }

                // For other payment methods, check status accordingly
                // For now, we'll just return not completed for other methods
                return Json(new { completed = false });
            }
            catch (Exception ex)
            {
                return Json(new { completed = false, message = $"Error checking payment status: {ex.Message}" });
            }
        }

        // Helper method to add purchased products to user library
        private async Task AddProductsToUserLibraryAsync(Order order)
        {
            var user = order.User;
            var orderItems = await _context.OrderItems.Where(oi => oi.OrderId == order.Id).ToListAsync();

            if (!orderItems.Any())
                return;

            var library = await _context.UserLibraries.FirstOrDefaultAsync(l => l.AppUserId == user.Id);

            if (library == null)
            {
                library = new UserLibrary
                {
                    AppUserId = user.Id,
                    Name = "My Games",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.UserLibraries.Add(library);
                await _context.SaveChangesAsync();
            }

            foreach (var item in orderItems)
            {
                var product = await _context.Product.FindAsync(item.ProductId);
                if (product != null)
                {
                    library.Products.Add(product);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> PaymentPending(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> MpesaCallback([FromBody] JsonElement body)
        {
            try
            {
                // Log the callback for debugging
                var callbackData = System.Text.Json.JsonSerializer.Serialize(body);
                // You could log this to a file or database for debugging

                // Extract the checkout request ID
                if (body.TryGetProperty("Body", out var bodyElement) &&
                    bodyElement.TryGetProperty("stkCallback", out var stkCallback) &&
                    stkCallback.TryGetProperty("CheckoutRequestID", out var checkoutRequestId) &&
                    stkCallback.TryGetProperty("ResultCode", out var resultCode))
                {
                    var requestId = checkoutRequestId.GetString();
                    var code = resultCode.GetInt32();

                    // Find the transaction with this checkout request ID
                    var transaction = await _context.Transactions
                        .Include(t => t.TransactionOrder)
                        .FirstOrDefaultAsync(t => t.ExternalReference == requestId);

                    if (transaction != null && transaction.TransactionOrder != null)
                    {
                        var order = transaction.TransactionOrder;

                        if (code == 0) // Success
                        {
                            // Update order and transaction
                            order.status = Order.Status.COMPLETED;
                            order.CompletedAt = DateTime.UtcNow;
                            transaction.PayStatus = Transaction.Status.COMPLETED;
                            transaction.UpdatedAt = DateTime.UtcNow;

                            _context.Update(order);
                            _context.Update(transaction);
                            await _context.SaveChangesAsync();

                            // Add products to user library
                            await AddProductsToUserLibraryAsync(order);
                        }
                        else // Failed
                        {
                            order.status = Order.Status.CANCELLED;
                            transaction.PayStatus = Transaction.Status.FAILED;

                            _context.Update(order);
                            _context.Update(transaction);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                // Return a success response to M-Pesa
                return Ok(new { ResultCode = 0, ResultDesc = "Success" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { ResultCode = 1, ResultDesc = "Error processing callback" });
            }
        }
    }
}