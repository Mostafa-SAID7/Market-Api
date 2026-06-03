using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds product categories
    /// </summary>
    public class CategorySeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<CategorySeeder> _logger;

        public CategorySeeder(MongoDbContext context, ILogger<CategorySeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding categories");
                throw;
            }
        }
    }
}
