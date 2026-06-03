using Market.API.Models.Entities;
using Market.API.Models.Enums;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds products for vendors
    /// </summary>
    public class ProductSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<ProductSeeder> _logger;

        public ProductSeeder(MongoDbContext context, ILogger<ProductSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding products");
                throw;
            }
        }
    }
}
