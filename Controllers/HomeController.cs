using Bingi_Storage.Data;
using Bingi_Storage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bingi_Storage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Product.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> TestProduct()
        {
            try
            {
                // First, let's check if we can connect to the database at all
                var connectionState = _context.Database.CanConnect();

                if (!connectionState)
                {
                    return Json(new { success = false, error = "Cannot connect to database" });
                }

                // Check if Product table exists by trying to query it
                var productCount = await _context.Product.CountAsync();

                // If we get here, the table exists
                return Json(new
                {
                    success = true,
                    count = productCount,
                    message = $"Found {productCount} products in database",
                    connectionState = "Connected"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace?.Split('\n')[0] // First line of stack trace
                });
            }
        }
    }
}
