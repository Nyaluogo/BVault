﻿using Bingi_Storage.Data;
using Bingi_Storage.Models;
using Bingi_Storage.ViewModels;
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

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _webHostEnvironment = hostEnv;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var viewModel = new ProductCreateViewModel();
            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
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
                            return View(viewModel);
                        }

                        // Validate file size (e.g., max 2MB)
                        const long maxFileSize = 2 * 1024 * 1024;
                        if (imageFile.Length > maxFileSize)
                        {
                            ModelState.AddModelError("ImageFile", "Image size must be less than 2MB.");
                            return View(viewModel);
                        }

                        // Generate unique file name and save file
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var fileName = $"{viewModel.Title}_{Guid.NewGuid()}{extension}";
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
                        return View(viewModel);
                    }
                    if (file.Length > maxPayloadFileSize)
                    {
                        ModelState.AddModelError("Payloads", $"File too large: {file.FileName}");
                        return View(viewModel);
                    }

                    // Ensure directory exists
                    var payloadUploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "products", "payloads");
                    if (!Directory.Exists(payloadUploadsFolder))
                        Directory.CreateDirectory(payloadUploadsFolder);

                    // Safe and unique file name
                    var safeTitle = string.Join("_", viewModel.Title.Split(Path.GetInvalidFileNameChars()));
                    var payloadFileName = $"{safeTitle}_{Guid.NewGuid()}{extension}";
                    var payloadFilePath = Path.Combine(payloadUploadsFolder, payloadFileName);

                    // Pseudocode plan:
                    // 1. For large file uploads, avoid loading the entire file into memory. Use streaming APIs and buffered reads/writes.
                    // 2. Use asynchronous file I/O (already present).
                    // 3. Optionally, increase request limits in Startup/Program if needed (not shown here).
                    // 4. For each payload file, use a buffer to copy the stream in chunks.
                    // 5. Use FileStreamOptions for .NET 9 for better performance and reliability.

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
                        UpdatedAt = DateTime.UtcNow
                    });
                }


                // Map ViewModel to Product entity
                var product = new Product
                {
                    Title = viewModel.Title,
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
                    // Set other properties as needed
                    Payloads = productPayloads,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Add(product);
                await _context.SaveChangesAsync();

                //redirect to edit page
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ShortDescription,Description,Price,FileSize,Version,ImageUrl,SystemRequirements,AgeRestriction,DownloadCount,AverageRating,TotalRatings,IsBettingEnabled,ProductPublishingStatus,ReleaseDate,SuspensionDate,DeleteDate,CreatedAt,UpdatedAt")] Product product)
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
