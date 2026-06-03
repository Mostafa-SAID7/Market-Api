using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Review collection indexes
    /// </summary>
    public class ReviewIndexes
    {
        private readonly IMongoCollection<Review> _collection;
        private readonly ILogger<ReviewIndexes> _logger;

        public ReviewIndexes(IMongoCollection<Review> collection, ILogger<ReviewIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // ProductId: retrieve product reviews
                var reviewProductIndex = new CreateIndexModel<Review>(
                    Builders<Review>.IndexKeys.Ascending(r => r.ProductId)
                );

                // CustomerId: retrieve user reviews
                var reviewCustomerIndex = new CreateIndexModel<Review>(
                    Builders<Review>.IndexKeys.Ascending(r => r.CustomerId)
                );

                await _collection.Indexes.CreateOneAsync(reviewProductIndex);
                await _collection.Indexes.CreateOneAsync(reviewCustomerIndex);
                _logger.LogDebug("Review indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("Review indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review indexes");
                throw;
            }
        }
    }
}
