using Bingi_Storage.Data;
using Bingi_Storage.Models;
using Bingi_Storage.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bingi_Storage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostEnv, UserManager<AppUser> userManager)
        {
            _context = context;
            _webHostEnvironment = hostEnv;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = _context.Product.Include(p => p.Publisher);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/MediaPlayer/5
        public async Task<IActionResult> MediaPlayer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var user = await GetCurrentUser();
            var viewModel = new ProductCreateViewModel();
            if (user != null)
            {
                viewModel._AppUser = user;
                var publisher = await _context.Publishers
                    .FirstOrDefaultAsync(p => p.AppUserId == user.Id);
                if (publisher != null)
                {
                    viewModel._Publisher = publisher;
                    // Pre-populate publisher data if exists
                    viewModel.Publisher.Name = publisher.Name;
                    viewModel.Publisher.Description = publisher.Description;
                    viewModel.Publisher.Email = publisher.Email;
                    viewModel.Publisher.Phone = publisher.Phone;
                    viewModel.Publisher.Country = publisher.Country;
                    viewModel.Publisher.RevenueShare = publisher.RevenueShare;
                }
            }

            return View(viewModel);
        }

        private async Task<AppUser?> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        // Helper method to repopulate ViewModel on validation failure
        private async Task<ProductCreateViewModel> RepopulateViewModel(ProductCreateViewModel viewModel)
        {
            var user = await GetCurrentUser();
            if (user != null)
            {
                viewModel._AppUser = user;
                var publisher = await _context.Publishers
                    .FirstOrDefaultAsync(p => p.AppUserId == user.Id);
                if (publisher != null)
                {
                    viewModel._Publisher = publisher;
                }
            }
            return viewModel;
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
        {
            // Get the currently authenticated user first
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            // Debug ModelState errors
            if (!ModelState.IsValid)
            {
                // Log all validation errors for debugging
                var errors = new List<string>();
                foreach (var modelError in ModelState)
                {
                    foreach (var error in modelError.Value.Errors)
                    {
                        var errorMessage = $"Field: {modelError.Key}, Error: {error.ErrorMessage}";
                        errors.Add(errorMessage);
                        System.Diagnostics.Debug.WriteLine(errorMessage);
                    }
                }

                // Add errors to TempData so you can see them in the view for debugging
                TempData["ValidationErrors"] = string.Join(" | ", errors);

                // Repopulate the viewModel and return with errors
                viewModel = await RepopulateViewModel(viewModel);
                return View(viewModel);
            }

            Publisher publisher = new Publisher();

            // Check if an image has been uploaded, validate, process, save, and get URL
            if (Request.Form.Files != null && Request.Form.Files.Count > 0)
            {
                var imageFile = Request.Form.Files["ImageFile"];
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validate file type (only allow jpg, png, jpeg)
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Only JPG and PNG images are allowed.");
                        viewModel = await RepopulateViewModel(viewModel);
                        return View(viewModel);
                    }

                    // Validate file size (e.g., max 2MB)
                    const long maxFileSize = 2 * 1024 * 1024;
                    if (imageFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError("ImageFile", "Image size must be less than 2MB.");
                        viewModel = await RepopulateViewModel(viewModel);
                        return View(viewModel);
                    }

                    // Generate unique file name and save file
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var fileName = $"{viewModel.Title?.Replace(" ", "_")}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    viewModel.ImageUrl = $"/uploads/products/{fileName}";
                }
            }

            // Check if payloads have been uploaded, validate, process, save, and get URL
            var productPayloads = new List<ProductPayload>();
            var allowedPayloadExtensions = new[] { ".zip", ".rar", ".iso", ".apk" };
            const long maxPayloadFileSize = 4L * 1024 * 1024 * 1024; // 4GB

            // Loop through all files in the request with the name "PayloadFileInput"
            var files = Request.Form.Files;
            foreach (IFormFile file in files)
            {
                if (file == null || file.Length == 0 || file.Name != "PayloadFileInput")
                    continue;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedPayloadExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Payloads", $"File type not allowed: {file.FileName}");
                    viewModel = await RepopulateViewModel(viewModel);
                    return View(viewModel);
                }
                if (file.Length > maxPayloadFileSize)
                {
                    ModelState.AddModelError("Payloads", $"File too large: {file.FileName}");
                    viewModel = await RepopulateViewModel(viewModel);
                    return View(viewModel);
                }

                // Ensure directory exists
                var payloadUploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products", "payloads");
                if (!Directory.Exists(payloadUploadsFolder))
                    Directory.CreateDirectory(payloadUploadsFolder);

                // Safe and unique file name
                var safeTitle = string.Join("_", (viewModel.Title ?? "product").Split(Path.GetInvalidFileNameChars()));
                var payloadFileName = $"{safeTitle}_{Guid.NewGuid()}{extension}";
                var payloadFilePath = Path.Combine(payloadUploadsFolder, payloadFileName);

                // Save file to disk using buffered streaming for large files
                var bufferSize = 1024 * 1024; // 1MB buffer
                var fileStreamOptions = new FileStreamOptions
                {
                    Access = FileAccess.Write,
                    Mode = FileMode.Create,
                    Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
                    BufferSize = bufferSize
                };
                using (var stream = new FileStream(payloadFilePath, fileStreamOptions))
                using (var inputStream = file.OpenReadStream())
                {
                    var buffer = new byte[bufferSize];
                    int bytesRead;
                    while ((bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await stream.WriteAsync(buffer.AsMemory(0, bytesRead));
                    }
                }

                // Add to payloads list
                productPayloads.Add(new ProductPayload
                {
                    Title = file.FileName,
                    FileSize = (file.Length / (1024.0 * 1024.0)).ToString("F2") + " MB",
                    FileUrl = $"/uploads/products/payloads/{payloadFileName}",
                    IsDemo = false, // Set from form if available
                    ShortDescription = "", // Set from form if available
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var existing_publisher = await _context.Publishers
                .FirstOrDefaultAsync(p => p.AppUser == currentUser);

            // Check if the publisher exists and if the current user owns it
            if (existing_publisher == null)
            {
                // If publisher does not exist, create a new one
                if (string.IsNullOrEmpty(viewModel.Publisher.Name))
                {
                    ModelState.AddModelError("Publisher.Name", "Publisher name is required.");
                    ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
                    viewModel = await RepopulateViewModel(viewModel);
                    return View(viewModel);
                }

                publisher = new Publisher
                {
                    Name = viewModel.Publisher.Name,
                    Description = viewModel.Publisher.Description,
                    Email = viewModel.Publisher.Email,
                    Phone = viewModel.Publisher.Phone,
                    Country = viewModel.Publisher.Country,
                    RevenueShare = viewModel.Publisher.RevenueShare,
                    AppUserId = currentUser.Id, // Assign the current user's ID
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Publishers.Add(publisher); // Add the new publisher to the context
                await _context.SaveChangesAsync(); // Save to get the Publisher ID
            }
            else
            {
                publisher = existing_publisher;
            }

            // Map ViewModel to Product entity
            var product = new Product
            {
                Title = viewModel.Title ?? "Untitled Product",
                CustomUrl = viewModel.CustomUrl,
                ShortDescription = viewModel.ShortDescription,
                Description = viewModel.Description,
                DefaultPrice = viewModel.DefaultPrice,
                SalePrice = viewModel.SalePrice,
                Discount = viewModel.Discount,
                Version = viewModel.Version,
                VideoTrailerUrl = viewModel.VideoTrailerUrl,
                ImageUrl = viewModel.ImageUrl,
                SystemRequirements = viewModel.SystemRequirements,
                Documentation = viewModel.Documentation,
                Tags = viewModel.Tags,
                Genre = viewModel.Genre,
                ExternalLinks = viewModel.ExternalLinks,
                AgeRestriction = viewModel.AgeRestriction,
                IsBettingEnabled = viewModel.IsBettingEnabled,
                IsAIGen = viewModel.IsAIGen,
                PricingState = (Product.PricingStatus)viewModel.PricingState,
                ProductPublishingStatus = (Product.PublishingStatus)viewModel.ProductPublishingStatus,
                ReleaseDate = viewModel.ReleaseDate,
                PublisherId = publisher.Id,
                Payloads = productPayloads,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Add(product);
            await _context.SaveChangesAsync();

            //redirect to edit page
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", product.PublisherId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ShortDescription,Description,DefaultPrice,SalePrice,Discount,FileSize,Version,ImageUrl,SystemRequirements,AgeRestriction,DownloadCount,AverageRating,TotalRatings,IsBettingEnabled,ProductPublishingStatus,ReleaseDate,SuspensionDate,DeleteDate,CreatedAt,UpdatedAt,PublisherId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", product.PublisherId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}