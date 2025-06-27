using Bingi_Storage.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bingi_Storage.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Bet> Bets { get; set; }
        public DbSet<BettingEvent> BettingEvents { get; set; }
        public DbSet<BettingMarket> BettingMarkets { get; set; }
        public DbSet<BettingOdds> BettingOdds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductMedia> ProductMedia { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserLibrary> UserLibraries { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserWallet> UserWallets { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserProfile <-> AppUser (1:1)
            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(up => up.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // UserWallet <-> AppUser (many:1)
            modelBuilder.Entity<UserWallet>()
                .HasOne(uw => uw.User)
                .WithMany(u => u.Wallets)
                .HasForeignKey(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction <-> UserWallet (many:1, restrict delete)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Wallet)
                .WithMany()
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction <-> PaymentMethod (many:1)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.PaymentMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction <-> AppUser (many:1)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Product <-> Publisher (many:1)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Publisher)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Product <-> ProductCategory (many:many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Category)
                .WithMany();

            // Product <-> ProductMedia (1:many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductMedia)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Order <-> AppUser (many:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Order <-> PaymentMethod (many:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.PaymentMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Order <-> Transaction (1:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderTransaction)
                .WithOne(t => t.TransactionOrder)
                .HasForeignKey<Order>(o => o.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order <-> OrderItems (1:many)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Bet <-> AppUser (many:1)
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Bet <-> BettingEvent (many:1)
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.BettingEvent)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Bet <-> BettingMarket (many:1)
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.BettingMarket)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Bet <-> BettingOdds (many:1)
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.Odds)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // ProductCategory seed data
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Id = 1, Name = "Action", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new ProductCategory { Id = 2, Name = "Adventure", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new ProductCategory { Id = 3, Name = "Racing", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new ProductCategory { Id = 4, Name = "Strategy", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new ProductCategory { Id = 5, Name = "Sport", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new ProductCategory { Id = 6, Name = "Casual", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new ProductCategory { Id = 7, Name = "Software", IsActive = true, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) }
            );

            //Product seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "BINGIMAN", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 2, Title = "Savage Gears", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 3, Title = "BINGIMAN 3", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 4, Title = "Debe", ShortDescription = "An exciting strategy game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 5, Title = "Political Rally", ShortDescription = "An exciting racing game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 6, Title = "Nafas", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 7, Title = "Political Fighter", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 8, Title = "Bingivision", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 9, Title = "Armed Rebellion", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", Price = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = 0, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) }
            );

            // Publisher seed data
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Default Publisher", PublicityStatus = Publisher.Status.VERIFIED, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) }
            );

            // PaymentMethod seed data
            //modelBuilder.Entity<PaymentMethod>().HasData(
            //    new PaymentMethod { Id = 1, Name = "Visa", PayType = PaymentMethod.Type.CARD, TransProvider = PaymentMethod.Provider.VISA, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            //    new PaymentMethod { Id = 2, Name = "MasterCard", PayType = PaymentMethod.Type.CARD, TransProvider = PaymentMethod.Provider.MASTERCARD, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            //    new PaymentMethod { Id = 3, Name = "PayPal", PayType = PaymentMethod.Type.E_WALLET, TransProvider = PaymentMethod.Provider.PAYPAL, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            //);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((Product)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((Product)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((Product)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((Product)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
