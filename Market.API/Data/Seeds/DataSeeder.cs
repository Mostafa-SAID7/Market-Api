using Market.API.Data;
using Market.API.Models.Entities;
using Market.API.Models.Enums;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds initial data into the database
    /// </summary>
    public class DataSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(MongoDbContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                await SeedCategoriesAsync();
                await SeedUsersAsync();
                await SeedVendorsAsync();
                await SeedProductsAsync();

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database seeding");
                throw;
            }
        }

        private async Task SeedCategoriesAsync()
        {
            var existingCategories = await _context.Categories.CountDocumentsAsync(FilterDefinition<Category>.Empty);
            if (existingCategories > 0)
            {
                _logger.LogInformation("Categories already exist. Skipping seeding.");
                return;
            }

            var categories = new List<Category>
            {
                Category.Create("Electronics", "Electronic devices and gadgets", null),
                Category.Create("Fashion", "Clothing, shoes, and accessories", null),
                Category.Create("Home & Garden", "Home and garden products", null),
                Category.Create("Books", "Books and educational materials", null)
            };

            await _context.Categories.InsertManyAsync(categories);
            _logger.LogInformation($"Seeded {categories.Count} categories");
        }

        private async Task SeedUsersAsync()
        {
            var existingUsers = await _context.Users.CountDocumentsAsync(FilterDefinition<User>.Empty);
            if (existingUsers > 0)
            {
                _logger.LogInformation("Users already exist. Skipping seeding.");
                return;
            }

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
                    Email = "vendor1@market.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Vendor@123"),
                    FirstName = "John",
                    LastName = "Vendor",
                    PhoneNumber = "+0987654321",
                    Role = UserRole.Vendor,
                    IsActive = true,
                    IsEmailVerified = true
                },
                new User
                {
                    Email = "customer1@market.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                    FirstName = "Jane",
                    LastName = "Customer",
                    PhoneNumber = "+1122334455",
                    Role = UserRole.Customer,
                    IsActive = true,
                    IsEmailVerified = true
                }
            };

            await _context.Users.InsertManyAsync(users);
            _logger.LogInformation($"Seeded {users.Count} users");
        }

        private async Task SeedVendorsAsync()
        {
            var existingVendors = await _context.Vendors.CountDocumentsAsync(FilterDefinition<Vendor>.Empty);
            if (existingVendors > 0)
            {
                _logger.LogInformation("Vendors already exist. Skipping seeding.");
                return;
            }

            var vendor1User = await _context.Users.Find(u => u.Email == "vendor1@market.com").FirstOrDefaultAsync();
            if (vendor1User == null)
            {
                _logger.LogWarning("Vendor user not found");
                return;
            }

            var vendors = new List<Vendor>
            {
                new Vendor
                {
                    UserId = vendor1User.Id,
                    StoreName = "Tech Paradise",
                    StoreDescription = "Premium electronics and gadgets",
                    Logo = "https://via.placeholder.com/200?text=Tech+Paradise",
                    Banner = "https://via.placeholder.com/1200x300?text=Tech+Paradise",
                    PhoneNumber = "+9876543210",
                    Address = "123 Tech Street",
                    City = "San Francisco",
                    Country = "USA",
                    ZipCode = "94105",
                    CommissionRate = 0.10m,
                    IsApproved = true,
                    IsActive = true,
                    AverageRating = 4.5
                }
            };

            await _context.Vendors.InsertManyAsync(vendors);
            _logger.LogInformation($"Seeded {vendors.Count} vendors");
        }

        private async Task SeedProductsAsync()
        {
            var existingProducts = await _context.Products.CountDocumentsAsync(FilterDefinition<Product>.Empty);
            if (existingProducts > 0)
            {
                _logger.LogInformation("Products already exist. Skipping seeding.");
                return;
            }

            var vendor = await _context.Vendors.Find(v => v.StoreName == "Tech Paradise").FirstOrDefaultAsync();
            if (vendor == null)
            {
                _logger.LogWarning("Vendor not found");
                return;
            }

            var products = new List<Product>
            {
                new Product
                {
                    VendorId = vendor.Id,
                    Name = "Wireless Headphones",
                    Description = "High-quality wireless headphones with noise cancellation",
                    ImageUrl = "https://via.placeholder.com/300?text=Wireless+Headphones",
                    Price = 129.99m,
                    Quantity = 50,
                    Category = "Electronics",
                    SubCategory = "Audio",
                    SKU = "WH-001",
                    Status = ProductStatus.Active,
                    AverageRating = 4.5,
                    ReviewCount = 15
                },
                new Product
                {
                    VendorId = vendor.Id,
                    Name = "Smartphone Stand",
                    Description = "Adjustable smartphone stand for desk",
                    ImageUrl = "https://via.placeholder.com/300?text=Phone+Stand",
                    Price = 19.99m,
                    Quantity = 200,
                    Category = "Electronics",
                    SubCategory = "Accessories",
                    SKU = "PS-001",
                    Status = ProductStatus.Active,
                    AverageRating = 4.2,
                    ReviewCount = 8
                },
                new Product
                {
                    VendorId = vendor.Id,
                    Name = "USB-C Charging Cable",
                    Description = "Durable USB-C charging cable with fast charging support",
                    ImageUrl = "https://via.placeholder.com/300?text=USB-C+Cable",
                    Price = 12.99m,
                    Quantity = 500,
                    Category = "Electronics",
                    SubCategory = "Cables",
                    SKU = "UC-001",
                    Status = ProductStatus.Active,
                    AverageRating = 4.7,
                    ReviewCount = 42
                }
            };

            // Add tags to products
            foreach (var product in products)
            {
                if (product.Name.Contains("Headphones"))
                    product.AddTags("wireless", "audio", "premium");
                else if (product.Name.Contains("Stand"))
                    product.AddTags("desk", "accessories", "portable");
                else if (product.Name.Contains("Cable"))
                    product.AddTags("charging", "usb-c", "fast-charge");
            }

            await _context.Products.InsertManyAsync(products);
            _logger.LogInformation($"Seeded {products.Count} products");
        }
    }
}
