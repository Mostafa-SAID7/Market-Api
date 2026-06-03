using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds product reviews and ratings
    /// </summary>
    public class ReviewSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<ReviewSeeder> _logger;

        public ReviewSeeder(MongoDbContext context, ILogger<ReviewSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                var existingReviews = await _context.Reviews.CountDocumentsAsync(FilterDefinition<Review>.Empty);
                if (existingReviews > 0)
                {
                    _logger.LogInformation("Reviews already exist. Skipping seeding.");
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

                var reviews = new List<Review>
                {
                    new Review
                    {
                        ProductId = products[0].Id, // Wireless Headphones
                        VendorId = vendor.Id,
                        CustomerId = customer.Id,
                        RatingValue = 5,
                        Title = "Excellent sound quality!",
                        Comment = "Amazing audio quality, noise cancellation is outstanding. Very comfortable to wear for long periods. Highly recommended!",
                        HelpfulCount = 24,
                        IsVerifiedPurchase = true
                    },
                    new Review
                    {
                        ProductId = products[1].Id, // Smartphone Stand
                        VendorId = vendor.Id,
                        CustomerId = customer.Id,
                        RatingValue = 4,
                        Title = "Good stand, could be sturdier",
                        Comment = "The stand works well and adjusts nicely, but sometimes feels a bit wobbly on uneven surfaces. Still a good value for the price.",
                        HelpfulCount = 8,
                        IsVerifiedPurchase = true
                    },
                    new Review
                    {
                        ProductId = products[2].Id, // USB-C Cable
                        VendorId = vendor.Id,
                        CustomerId = customer.Id,
                        RatingValue = 5,
                        Title = "Fast charging, durable cable",
                        Comment = "Fantastic cable! Fast charging speed is incredible. Build quality feels premium and it's lasted longer than other cables I've used. Best USB-C cable I own.",
                        HelpfulCount = 42,
                        IsVerifiedPurchase = true
                    }
                };

                await _context.Reviews.InsertManyAsync(reviews);
                _logger.LogInformation($"Seeded {reviews.Count} reviews");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding reviews");
                throw;
            }
        }
    }
}
