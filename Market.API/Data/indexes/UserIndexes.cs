using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// User collection indexes
    /// </summary>
    public class UserIndexes
    {
        private readonly IMongoCollection<User> _collection;
        private readonly ILogger<UserIndexes> _logger;

        public UserIndexes(IMongoCollection<User> collection, ILogger<UserIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // Email: unique index for fast user lookup
                var userEmailIndex = new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(u => u.Email),
                    new CreateIndexOptions { Unique = true }
                );

                await _collection.Indexes.CreateOneAsync(userEmailIndex);
                _logger.LogDebug("User indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("User indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user indexes");
                throw;
            }
        }
    }
}
