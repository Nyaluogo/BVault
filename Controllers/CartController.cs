using Bingi_Storage.Data;
using Bingi_Storage.Models;
using Bingi_Storage.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bingi_Storage.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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
            if (!ModelState.IsValid)
                return View("Checkout", model);

            var cart = await GetOrCreateCartAsync();
            var user = await _userManager.GetUserAsync(User);

            // Get the selected wallet
            var wallet = await _context.UserWallets.FindAsync(model.SelectedWalletId);

            if (wallet == null || wallet.UserId != user.Id)
            {
                ModelState.AddModelError("", "Invalid wallet selected.");
                return View("Checkout", model);
            }

            // Check if sufficient funds
            var cartWithProducts = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            decimal totalAmount = cartWithProducts.Total;

            if (wallet.Balance < totalAmount)
            {
                ModelState.AddModelError("", "Insufficient funds in the selected wallet.");
                return View("Checkout", model);
            }

            // Create transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Name = "Game Purchase",
                Description = $"Purchase of {cartWithProducts.Items.Count} item(s)",
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
                Description = $"Purchase of {cartWithProducts.Items.Count} item(s)",
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
            var orderItems = cartWithProducts.Items.Select(item => new OrderItem
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
                    foreach (var item in cartWithProducts.Items)
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
    }
}