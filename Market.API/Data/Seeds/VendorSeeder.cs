using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds vendor stores
    /// </summary>
    public class VendorSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<VendorSeeder> _logger;

        public VendorSeeder(MongoDbContext context, ILogger<VendorSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding vendors");
                throw;
            }
        }
    }
}
