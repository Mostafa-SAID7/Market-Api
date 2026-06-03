using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Orchestrator for database indexes - coordinates individual entity index creation
    /// </summary>
    public class IndexConfiguration
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<IndexConfiguration> _logger;

        public IndexConfiguration(MongoDbContext context, ILogger<IndexConfiguration> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Create all collection indexes in dependency order
        /// </summary>
        public async Task CreateAllIndexesAsync()
        {
            _logger.LogInformation("Creating database indexes...");

            await CreateUserIndexesAsync();
            await CreateCategoryIndexesAsync();
            await CreateVendorIndexesAsync();
            await CreateProductIndexesAsync();
            await CreateOrderIndexesAsync();
            await CreateReviewIndexesAsync();
            await CreateCartIndexesAsync();

            _logger.LogInformation("All indexes created successfully");
        }

        private async Task CreateUserIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<UserIndexes>();
            var indexes = new UserIndexes(_context.Users, logger);
            await indexes.CreateIndexesAsync();
        }

        private async Task CreateCategoryIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<CategoryIndexes>();
            var indexes = new CategoryIndexes(_context.Categories, logger);
            await indexes.CreateIndexesAsync();
        }

        private async Task CreateVendorIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<VendorIndexes>();
            var indexes = new VendorIndexes(_context.Vendors, logger);
            await indexes.CreateIndexesAsync();
        }

        private async Task CreateProductIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<ProductIndexes>();
            var indexes = new ProductIndexes(_context.Products, logger);
            await indexes.CreateIndexesAsync();
        }

        private async Task CreateOrderIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<OrderIndexes>();
            var indexes = new OrderIndexes(_context.Orders, logger);
            await indexes.CreateIndexesAsync();
        }

        private async Task CreateReviewIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<ReviewIndexes>();
            var indexes = new ReviewIndexes(_context.Reviews, logger);
            await indexes.CreateIndexesAsync();
        }

        private async Task CreateCartIndexesAsync()
        {
            var logger = LoggerFactory.Create(config => config.AddConsole())
                .CreateLogger<CartIndexes>();
            var indexes = new CartIndexes(_context.Carts, logger);
            await indexes.CreateIndexesAsync();
        }
    }
}
