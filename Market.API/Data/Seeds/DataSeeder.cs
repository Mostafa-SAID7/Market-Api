using Market.API.Models.Entities;
using Market.API.Models.Enums;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds initial data into the database
    /// </summary>
    public class DataSeeder
    {
        private readonly MarketDbContext _context;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(MarketDbContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seed data if database is empty
        /// </summary>
        public async Task SeedAsync()
        {
            try
            {
                // Only seed if database is empty
                if (_context.Users.Any())
                {
                    _logger.LogInformation("Database already seeded. Skipping seed operation.");
                    return;
                }

                _logger.LogInformation("Starting database seeding...");

                // Seed users
                await SeedUsersAsync();
                await _context.SaveChangesAsync();

                // Seed categories
                await SeedCategoriesAsync();
                await _context.SaveChangesAsync();

                // Seed vendors
                await SeedVendorsAsync();
                await _context.SaveChangesAsync();

                // Seed products
                await SeedProductsAsync();
                await _context.SaveChangesAsync();

                // Seed carts
                await SeedCartsAsync();
                await _context.SaveChangesAsync();

                // Seed orders
                await SeedOrdersAsync();
                await _context.SaveChangesAsync();

                // Seed reviews
                await SeedReviewsAsync();
                await _context.SaveChangesAsync();

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding database");
                throw;
            }
        }

        private async Task SeedUsersAsync()
        {
            _logger.LogInformation("Seeding users...");

            var users = new List<User>
            {
                new User
                {
                    Email = "admin@market.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    FirstName = "Admin",
                    LastName = "User",
                    PhoneNumber = "+1234567890",
                    Role = UserRole.Admin,
                    IsActive = true,
                    IsEmailVerified = true
                },
                new User
                {
                    Email = "vendor@market.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Vendor@123"),
                    FirstName = "Vendor",
                    LastName = "User",
                    PhoneNumber = "+1234567891",
                    Role = UserRole.Vendor,
                    IsActive = true,
                    IsEmailVerified = true
                },
                new User
                {
                    Email = "customer@market.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                    FirstName = "Customer",
                    LastName = "User",
                    PhoneNumber = "+1234567892",
                    Role = UserRole.Customer,
                    IsActive = true,
                    IsEmailVerified = true
                }
            };

            await _context.Users.AddRangeAsync(users);
        }

        private async Task SeedCategoriesAsync()
        {
            _logger.LogInformation("Seeding categories...");

            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Electronics",
                    Description = "Electronic devices and gadgets",
                    Slug = "electronics",
                    IsActive = true,
                    DisplayOrder = 1
                },
                new Category
                {
                    Name = "Clothing",
                    Description = "Apparel and clothing items",
                    Slug = "clothing",
                    IsActive = true,
                    DisplayOrder = 2
                },
                new Category
                {
                    Name = "Books",
                    Description = "Books and reading materials",
                    Slug = "books",
                    IsActive = true,
                    DisplayOrder = 3
                }
            };

            await _context.Categories.AddRangeAsync(categories);
        }

        private async Task SeedVendorsAsync()
        {
            _logger.LogInformation("Seeding vendors...");

            var vendor = new Vendor
            {
                UserId = 2, // vendor@market.com user
                StoreName = "Tech Store",
                StoreDescription = "Official tech store",
                PhoneNumber = "+1234567891",
                CommissionRate = 0.10m,
                IsApproved = true,
                IsActive = true,
                AverageRating = 4.5
            };

            await _context.Vendors.AddAsync(vendor);
        }

        private async Task SeedProductsAsync()
        {
            _logger.LogInformation("Seeding products...");

            var product = new Product
            {
                VendorId = 1, // First vendor
                CategoryId = 1, // Electronics
                Name = "Wireless Headphones",
                Description = "High-quality wireless headphones",
                Price = 99.99m,
                Quantity = 100,
                SKU = "WH-001",
                Status = ProductStatus.Active,
                AverageRating = 4.5,
                ReviewCount = 10
            };

            await _context.Products.AddAsync(product);
        }

        private async Task SeedCartsAsync()
        {
            _logger.LogInformation("Seeding carts...");

            var cart = new Cart
            {
                UserId = 3 // customer@market.com user
            };

            await _context.Carts.AddAsync(cart);
        }

        private async Task SeedOrdersAsync()
        {
            _logger.LogInformation("Seeding orders...");

            var order = new Order
            {
                CustomerId = 3, // customer@market.com user
                OrderNumber = Order.GenerateOrderNumber(),
                SubTotal = 99.99m,
                ShippingCost = 10.00m,
                Tax = 8.80m,
                TotalPrice = 118.79m,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                ShippingAddress = "123 Main Street"
            };

            await _context.Orders.AddAsync(order);
        }

        private async Task SeedReviewsAsync()
        {
            _logger.LogInformation("Seeding reviews...");

            var review = new Review
            {
                ProductId = 1,
                VendorId = 1,
                CustomerId = 3, // customer@market.com user
                RatingValue = 5,
                Title = "Excellent product",
                Comment = "Great quality and fast delivery",
                IsVerifiedPurchase = true
            };

            await _context.Reviews.AddAsync(review);
        }
    }
}
