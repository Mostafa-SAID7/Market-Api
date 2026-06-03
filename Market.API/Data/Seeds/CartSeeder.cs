using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds shopping carts for customers
    /// </summary>
    public class CartSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<CartSeeder> _logger;

        public CartSeeder(MongoDbContext context, ILogger<CartSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                var existingCarts = await _context.Carts.CountDocumentsAsync(FilterDefinition<Cart>.Empty);
                if (existingCarts > 0)
                {
                    _logger.LogInformation("Carts already exist. Skipping seeding.");
                    return;
                }

                var customer = await _context.Users.Find(u => u.Email == "customer1@market.com").FirstOrDefaultAsync();
                if (customer == null)
                {
                    _logger.LogWarning("Customer user not found");
                    return;
                }

                var vendor = await _context.Vendors.Find(v => v.StoreName == "Tech Paradise").FirstOrDefaultAsync();
                if (vendor == null)
                {
                    _logger.LogWarning("Vendor not found");
                    return;
                }

                var products = await _context.Products.Find(p => p.VendorId == vendor.Id).ToListAsync();
                if (products.Count == 0)
                {
                    _logger.LogWarning("Products not found");
                    return;
                }

                var carts = new List<Cart>
                {
                    new Cart
                    {
                        UserId = customer.Id,
                        Items = new List<CartItem>
                        {
                            new CartItem
                            {
                                ProductId = products[0].Id, // Wireless Headphones
                                ProductName = products[0].Name,
                                VendorId = vendor.Id,
                                Price = products[0].Price,
                                Quantity = 1,
                                ImageUrl = products[0].ImageUrl
                            },
                            new CartItem
                            {
                                ProductId = products[2].Id, // USB-C Cable
                                ProductName = products[2].Name,
                                VendorId = vendor.Id,
                                Price = products[2].Price,
                                Quantity = 2,
                                ImageUrl = products[2].ImageUrl
                            }
                        }
                    }
                };

                await _context.Carts.InsertManyAsync(carts);
                _logger.LogInformation($"Seeded shopping carts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding carts");
                throw;
            }
        }
    }
}
