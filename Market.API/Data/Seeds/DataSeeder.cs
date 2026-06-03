using Market.API.Data;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Orchestrator for database seeding - coordinates individual entity seeders
    /// </summary>
    public class DataSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<DataSeeder> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DataSeeder(MongoDbContext context, ILogger<DataSeeder> logger, IServiceProvider serviceProvider)
        {
            _context = context;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Execute all seeders in dependency order
        /// </summary>
        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                // Seed in dependency order
                await SeedCategoriesAsync();
                await SeedUsersAsync();
                await SeedVendorsAsync();
                await SeedProductsAsync();
                await SeedOrdersAsync();
                await SeedReviewsAsync();
                await SeedCartsAsync();

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
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CategorySeeder>>();
                var seeder = new CategorySeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }

        private async Task SeedUsersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserSeeder>>();
                var seeder = new UserSeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }

        private async Task SeedVendorsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<VendorSeeder>>();
                var seeder = new VendorSeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }

        private async Task SeedProductsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductSeeder>>();
                var seeder = new ProductSeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }

        private async Task SeedOrdersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderSeeder>>();
                var seeder = new OrderSeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }

        private async Task SeedReviewsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ReviewSeeder>>();
                var seeder = new ReviewSeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }

        private async Task SeedCartsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CartSeeder>>();
                var seeder = new CartSeeder(_context, logger);
                await seeder.SeedAsync();
            }
        }
    }
}
