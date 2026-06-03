using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Cart collection indexes
    /// </summary>
    public class CartIndexes
    {
        private readonly IMongoCollection<Cart> _collection;
        private readonly ILogger<CartIndexes> _logger;

        public CartIndexes(IMongoCollection<Cart> collection, ILogger<CartIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // UserId: unique cart per user
                var cartUserIndex = new CreateIndexModel<Cart>(
                    Builders<Cart>.IndexKeys.Ascending(c => c.UserId),
                    new CreateIndexOptions { Unique = true, Sparse = true }
                );

                await _collection.Indexes.CreateOneAsync(cartUserIndex);
                _logger.LogDebug("Cart indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("Cart indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart indexes");
                throw;
            }
        }
    }
}
