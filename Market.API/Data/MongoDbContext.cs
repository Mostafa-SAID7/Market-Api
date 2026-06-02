using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// MongoDB context - manages database collections and indexes
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbContext> _logger;

        public MongoDbContext(IOptions<MongoDbSettings> settings, ILogger<MongoDbContext> logger)
        {
            _logger = logger;
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        #region Collections

        /// <summary>
        /// Users collection
        /// </summary>
        public IMongoCollection<User> Users => _database.GetCollection<User>(nameof(User));

        /// <summary>
        /// Vendors collection
        /// </summary>
        public IMongoCollection<Vendor> Vendors => _database.GetCollection<Vendor>(nameof(Vendor));

        /// <summary>
        /// Products collection
        /// </summary>
        public IMongoCollection<Product> Products => _database.GetCollection<Product>(nameof(Product));

        /// <summary>
        /// Categories collection
        /// </summary>
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>(nameof(Category));

        /// <summary>
        /// Orders collection
        /// </summary>
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>(nameof(Order));

        /// <summary>
        /// Reviews collection
        /// </summary>
        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>(nameof(Review));

        /// <summary>
        /// Shopping carts collection
        /// </summary>
        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>(nameof(Cart));

        #endregion

        /// <summary>
        /// Initialize database - create indexes
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Initializing MongoDB indexes...");
                await CreateIndexesAsync();
                _logger.LogInformation("MongoDB indexes initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing MongoDB indexes");
                throw;
            }
        }

        /// <summary>
        /// Create all collection indexes
        /// </summary>
        private async Task CreateIndexesAsync()
        {
            try
            {
                await CreateUserIndexesAsync();
                await CreateProductIndexesAsync();
                await CreateOrderIndexesAsync();
                await CreateReviewIndexesAsync();
                await CreateCartIndexesAsync();
                await CreateCategoryIndexesAsync();
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index"))
            {
                _logger.LogInformation("Indexes already exist in database, skipping creation");
            }
        }

        /// <summary>
        /// Create User collection indexes
        /// </summary>
        private async Task CreateUserIndexesAsync()
        {
            var userEmailIndex = new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(u => u.Email),
                new CreateIndexOptions { Unique = true }
            );
            await Users.Indexes.CreateOneAsync(userEmailIndex);
            _logger.LogDebug("User indexes created");
        }

        /// <summary>
        /// Create Product collection indexes
        /// </summary>
        private async Task CreateProductIndexesAsync()
        {
            var productVendorIndex = new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p => p.VendorId)
            );

            var productCategoryIndex = new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p => p.Category)
            );

            var productNotDeletedIndex = new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p => p.IsDeleted)
            );

            await Products.Indexes.CreateOneAsync(productVendorIndex);
            await Products.Indexes.CreateOneAsync(productCategoryIndex);
            await Products.Indexes.CreateOneAsync(productNotDeletedIndex);
            _logger.LogDebug("Product indexes created");
        }

        /// <summary>
        /// Create Order collection indexes
        /// </summary>
        private async Task CreateOrderIndexesAsync()
        {
            var orderCustomerIndex = new CreateIndexModel<Order>(
                Builders<Order>.IndexKeys.Ascending(o => o.CustomerId)
            );

            var orderNumberIndex = new CreateIndexModel<Order>(
                Builders<Order>.IndexKeys.Ascending(o => o.OrderNumber),
                new CreateIndexOptions { Unique = true, Sparse = true }
            );

            await Orders.Indexes.CreateOneAsync(orderCustomerIndex);
            await Orders.Indexes.CreateOneAsync(orderNumberIndex);
            _logger.LogDebug("Order indexes created");
        }

        /// <summary>
        /// Create Review collection indexes
        /// </summary>
        private async Task CreateReviewIndexesAsync()
        {
            var reviewProductIndex = new CreateIndexModel<Review>(
                Builders<Review>.IndexKeys.Ascending(r => r.ProductId)
            );

            var reviewCustomerIndex = new CreateIndexModel<Review>(
                Builders<Review>.IndexKeys.Ascending(r => r.CustomerId)
            );

            await Reviews.Indexes.CreateOneAsync(reviewProductIndex);
            await Reviews.Indexes.CreateOneAsync(reviewCustomerIndex);
            _logger.LogDebug("Review indexes created");
        }

        /// <summary>
        /// Create Cart collection indexes
        /// </summary>
        private async Task CreateCartIndexesAsync()
        {
            var cartUserIndex = new CreateIndexModel<Cart>(
                Builders<Cart>.IndexKeys.Ascending(c => c.UserId),
                new CreateIndexOptions { Unique = true, Sparse = true }
            );

            await Carts.Indexes.CreateOneAsync(cartUserIndex);
            _logger.LogDebug("Cart indexes created");
        }

        /// <summary>
        /// Create Category collection indexes
        /// </summary>
        private async Task CreateCategoryIndexesAsync()
        {
            var categorySlugIndex = new CreateIndexModel<Category>(
                Builders<Category>.IndexKeys.Ascending(c => c.SlugValue),
                new CreateIndexOptions { Unique = true, Sparse = true }
            );

            await Categories.Indexes.CreateOneAsync(categorySlugIndex);
            _logger.LogDebug("Category indexes created");
        }
    }
}
