using Market.API.Data.Indexes;
using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// MongoDB context - manages database collections
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbContext> _logger;
        private readonly ILogger<IndexConfiguration> _indexLogger;

        public MongoDbContext(IOptions<MongoDbSettings> settings, ILogger<MongoDbContext> logger, ILogger<IndexConfiguration> indexLogger)
        {
            _logger = logger;
            _indexLogger = indexLogger;
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
                var indexConfiguration = new IndexConfiguration(this, _indexLogger);
                await indexConfiguration.CreateAllIndexesAsync();
                _logger.LogInformation("MongoDB indexes initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing MongoDB indexes");
                throw;
            }
        }
    }
}
