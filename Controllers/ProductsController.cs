using Bingi_Storage.Data;
using Bingi_Storage.Models;
using Bingi_Storage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bingi_Storage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
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
