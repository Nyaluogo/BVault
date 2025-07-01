using Bingi_Storage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bingi_Storage.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
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
        public DbSet<ProductPayload> ProductPayload { get; set; }
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

            // AppUser <-> Publisher (1:0..1)
            modelBuilder.Entity<AppUser>()
                .HasOne(a => a.Publisher)
                .WithOne(p => p.AppUser)
                .HasForeignKey<Publisher>(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppUser <-> UserLibrary (1:1)
            modelBuilder.Entity<AppUser>()
                .HasOne(a => a.Library)
                .WithOne(l => l.AppUser)
                .HasForeignKey<UserLibrary>(l => l.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserLibrary <-> Product (many:many)
            modelBuilder.Entity<UserLibrary>()
                .HasMany(l => l.Products)
                .WithMany();

            // AppUser <-> UserProfile (1:1)
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Profile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId) // Fixed: Using UserId instead of Id
                .OnDelete(DeleteBehavior.Cascade);

            //AppUser <-> UserWallet (1:many)
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Wallets)
                .WithOne(uw => uw.User)
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
                .WithMany(pub => pub.Products)
                .HasForeignKey(p => p.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product <-> ProductCategory (many:many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Category)
                .WithMany();

            // Product <-> ProductMedia (1:many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Media)
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product <-> ProductPayload (1:many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Payloads)
                .WithOne(pl => pl.Product)
                .HasForeignKey(pl => pl.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product <-> Product (self-referencing one-to-many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ChildProducts)
                .WithOne(p => p.ParentProduct)
                .HasForeignKey(p => p.ParentProductId)
                .IsRequired(false);

            // Wishlist <-> Product (many-to-many)
            modelBuilder.Entity<Wishlist>()
                .HasMany(w => w.Products)
                .WithMany();

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


            // Configure decimal precision for UserWallet.Balance
            modelBuilder.Entity<UserWallet>()
                .Property(w => w.Balance)
                .HasPrecision(18, 2);

            SeedUsers(modelBuilder);

            var utcNow = new DateTime(2025, 7, 1, 10, 0, 0, DateTimeKind.Utc);

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

            // Publisher seed data
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, AppUserId = "a18be9c0-aa65-4af8-bd17-00bd9344e575", Name = "Nyabingi Studio", PublicityStatus = Publisher.Status.VERIFIED, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) }
            );

            //Product seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, PublisherId = 1, Title = "BINGIMAN", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://nyabingi.co.ke/index/wp-content/uploads/2025/02/Screenshot-2025-02-18-163014.png", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 2, PublisherId = 1, Title = "Savage Gears", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://nyabingi.co.ke/index/wp-content/uploads/2025/02/SavageGearsOfficialPoster-1-scaled.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 3, PublisherId = 1, Title = "BINGIMAN 3", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 4, PublisherId = 1, Title = "Debe", ShortDescription = "An exciting strategy game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 5, PublisherId = 1, Title = "Political Rally", ShortDescription = "An exciting racing game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 6, PublisherId = 1, Title = "Nafas", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://nyabingi.co.ke/index/wp-content/uploads/2025/02/Screenshot-2025-02-18-163208.png", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 7, PublisherId = 1, Title = "Political Fighter", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 8, PublisherId = 1, Title = "Bingivision", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) },
                new Product { Id = 9, PublisherId = 1, Title = "Armed Rebellion", ShortDescription = "An exciting action game.", Description = "This is a detailed description of the sample game.", SalePrice = 29.99m, FileSize = 5.0m, Version = 1.0m, ImageUrl = "https://example.com/sample-game.jpg", SystemRequirements = "Windows 10 or higher", AgeRestriction = 18, DownloadCount = 1000, AverageRating = 4.5m, TotalRatings = 200, IsBettingEnabled = false, ProductPublishingStatus = Models.Product.PublishingStatus.DRAFT, CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc) }
            );

            

            // PaymentMethod seed data
            //modelBuilder.Entity<PaymentMethod>().HasData(
            //    new PaymentMethod { Id = 1, Name = "Visa", PayType = PaymentMethod.Type.CARD, TransProvider = PaymentMethod.Provider.VISA, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            //    new PaymentMethod { Id = 2, Name = "MasterCard", PayType = PaymentMethod.Type.CARD, TransProvider = PaymentMethod.Provider.MASTERCARD, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            //    new PaymentMethod { Id = 3, Name = "PayPal", PayType = PaymentMethod.Type.E_WALLET, TransProvider = PaymentMethod.Provider.PAYPAL, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            //);
        }



        public void SeedUsers(ModelBuilder modelBuilder)
        {
            const string ADMIN_ROLE_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string USER_ROLE_ID = "ad376a8f-9eab-4bb9-9fca-30b01540f445";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ADMIN_ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = USER_ROLE_ID,
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // Use static password hashes instead of dynamic generation
            const string ADMIN_USER_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e576";
            var adminUser = new AppUser
            {
                Id = ADMIN_USER_ID,
                UserName = "Alpha",
                NormalizedUserName = "ALPHA",
                Email = "edwinnyaluogo@gmail.com",
                NormalizedEmail = "EDWINNYALUOGO@GMAIL.COM",
                EmailConfirmed = true,
                // Static hash for password "q1w2e3r4P+"
                PasswordHash = "AQAAAAIAAYagAAAAEOBV6cpmXcJxwJ+mw+fPg4x8aPgKHC9veTzthQGcdqeacsNMFOSjQf45M9Cf7rcNyw==",
                SecurityStamp = "a18be9c0-aa65-4af8-bd17-00bd9344e576",
                ConcurrencyStamp = "admin-concurrency-stamp-001", // Add static ConcurrencyStamp
                IsAdmin = true,
                IsSuperAdmin = true,
                DateOfRegistration = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                DateOfLastLogin = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
            };

            const string USER1_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e577";
            var user1 = new AppUser
            {
                Id = USER1_ID,
                UserName = "COMMANDER",
                NormalizedUserName = "COMMANDER",
                Email = "nyabingistudio@proton.me",
                NormalizedEmail = "NYABINGISTUDIO@PROTON.ME",
                EmailConfirmed = true,
                // Static hash for password "q1w2e3r4P+"
                PasswordHash = "AQAAAAIAAYagAAAAELSOzm3DdIyV6qYy7JLce6I+HZAB4Wu9yip6c7K0xnl3WN4mTfuLFRk5DQKM764h2w==",
                SecurityStamp = "a18be9c0-aa65-4af8-bd17-00bd9344e577",
                ConcurrencyStamp = "user1-concurrency-stamp-001", // Add static ConcurrencyStamp
                DateOfRegistration = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                DateOfLastLogin = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
            };

            const string USER2_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e578";
            var user2 = new AppUser
            {
                Id = USER2_ID,
                UserName = "user2@bingi.com",
                NormalizedUserName = "USER2@BINGI.COM",
                Email = "user2@bingi.com",
                NormalizedEmail = "USER2@BINGI.COM",
                EmailConfirmed = true,
                // Static hash for password "P@ssword1."
                PasswordHash = "AQAAAAIAAYagAAAAEMI+8KKpvzMrq2MQ1WC+9BpylGx5Fqy/LXoucCCaLKeTQdkPRbKrUiNlxeQz766VrA==",
                SecurityStamp = "a18be9c0-aa65-4af8-bd17-00bd9344e578",
                ConcurrencyStamp = "user2-concurrency-stamp-001", // Add static ConcurrencyStamp
                DateOfRegistration = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                DateOfLastLogin = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
            };

            const string USER3_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e579";
            var user3 = new AppUser
            {
                Id = USER3_ID,
                UserName = "user3@bingi.com",
                NormalizedUserName = "USER3@BINGI.COM",
                Email = "user3@bingi.com",
                NormalizedEmail = "USER3@BINGI.COM",
                EmailConfirmed = true,
                // Static hash for password "P@ssword1."
                PasswordHash = "AQAAAAIAAYagAAAAENcM3j68sDaodsrHTXxTk3c2KWkafkWe3VEpDl0Ty1B9k76RUUTnU+oLhmEKkDR/Jg==",
                SecurityStamp = "a18be9c0-aa65-4af8-bd17-00bd9344e579",
                ConcurrencyStamp = "user3-concurrency-stamp-001", // Add static ConcurrencyStamp
                DateOfRegistration = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                DateOfLastLogin = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
            };

            modelBuilder.Entity<AppUser>().HasData(adminUser, user1, user2, user3);

            // Seed UserProfile data 
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile
                {
                    Id = 1,
                    UserId = ADMIN_USER_ID,
                    Bio = "Welcome to Alpha's profile!",
                    Country = "Not specified",
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                },
                new UserProfile
                {
                    Id = 2,
                    UserId = USER1_ID,
                    Bio = "Welcome to COMMANDER's profile!",
                    Country = "Not specified",
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                },
                new UserProfile
                {
                    Id = 3,
                    UserId = USER2_ID,
                    Bio = "Welcome to user2@bingi.com's profile!",
                    Country = "Not specified",
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                },
                new UserProfile
                {
                    Id = 4,
                    UserId = USER3_ID,
                    Bio = "Welcome to user3@bingi.com's profile!",
                    Country = "Not specified",
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                }
            );

            //seed a default wallet for each user
            modelBuilder.Entity<UserWallet>().HasData(
                new UserWallet
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Static Guid instead of int
                    UserId = ADMIN_USER_ID,
                    Name = "Alpha's Wallet",
                    Balance = 1000.00m,
                    Currency = "KES",
                    WalletStatus = UserWallet.Status.ACTIVE,
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                },
                new UserWallet
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Static Guid instead of int
                    UserId = USER1_ID,
                    Name = "COMMANDER's Wallet",
                    Balance = 500.00m,
                    Currency = "KES",
                    WalletStatus = UserWallet.Status.ACTIVE,
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                },
                new UserWallet
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Static Guid instead of int
                    UserId = USER2_ID,
                    Name = "Default Wallet",
                    Balance = 500.00m,
                    Currency = "KES",
                    WalletStatus = UserWallet.Status.ACTIVE,
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                },
                new UserWallet
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), // Static Guid instead of int
                    UserId = USER3_ID,
                    Name = "Default Wallet",
                    Balance = 500.00m,
                    Currency = "KES",
                    WalletStatus = UserWallet.Status.ACTIVE,
                    CreatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 6, 26, 16, 46, 50, DateTimeKind.Utc)
                }
            );



            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_USER_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = USER_ROLE_ID,
                    UserId = USER1_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = USER_ROLE_ID,
                    UserId = USER2_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = USER_ROLE_ID,
                    UserId = USER3_ID
                }
            );
        }


        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Product product)
                {
                    if (entry.State == EntityState.Added)
                    {
                        product.CreatedAt = DateTime.UtcNow;
                    }
                    product.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is ProductPayload payload)
                {
                    if (entry.State == EntityState.Added)
                    {
                        payload.CreatedAt = DateTime.UtcNow;
                    }
                    payload.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is UserLibrary library)
                {
                    if (entry.State == EntityState.Added)
                    {
                        library.CreatedAt = DateTime.UtcNow;
                    }
                    library.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is UserProfile profile)
                {
                    if (entry.State == EntityState.Added)
                    {
                        profile.CreatedAt = DateTime.UtcNow;
                    }
                    profile.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is UserWallet wallet)
                {
                    if (entry.State == EntityState.Added)
                    {
                        wallet.CreatedAt = DateTime.UtcNow;
                    }
                    wallet.UpdatedAt = DateTime.UtcNow;
                }
                // Add other entities as needed
            }
        }
    }
}