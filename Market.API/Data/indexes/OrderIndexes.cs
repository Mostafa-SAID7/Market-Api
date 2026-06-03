using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Order collection indexes
    /// </summary>
    public class OrderIndexes
    {
        private readonly IMongoCollection<Order> _collection;
        private readonly ILogger<OrderIndexes> _logger;

        public OrderIndexes(IMongoCollection<Order> collection, ILogger<OrderIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // CustomerId: retrieve user order history
                var orderCustomerIndex = new CreateIndexModel<Order>(
                    Builders<Order>.IndexKeys.Ascending(o => o.CustomerId)
                );

                // OrderNumber: unique order identification
                var orderNumberIndex = new CreateIndexModel<Order>(
                    Builders<Order>.IndexKeys.Ascending(o => o.OrderNumber),
                    new CreateIndexOptions { Unique = true, Sparse = true }
                );

                await _collection.Indexes.CreateOneAsync(orderCustomerIndex);
                await _collection.Indexes.CreateOneAsync(orderNumberIndex);
                _logger.LogDebug("Order indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("Order indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order indexes");
                throw;
            }
        }
    }
}
