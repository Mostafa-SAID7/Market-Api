using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Category collection indexes
    /// </summary>
    public class CategoryIndexes
    {
        private readonly IMongoCollection<Category> _collection;
        private readonly ILogger<CategoryIndexes> _logger;

        public CategoryIndexes(IMongoCollection<Category> collection, ILogger<CategoryIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // SlugValue: unique category slug for URL-friendly lookups
                var categorySlugIndex = new CreateIndexModel<Category>(
                    Builders<Category>.IndexKeys.Ascending(c => c.SlugValue),
                    new CreateIndexOptions { Unique = true, Sparse = true }
                );

                await _collection.Indexes.CreateOneAsync(categorySlugIndex);
                _logger.LogDebug("Category indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("Category indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category indexes");
                throw;
            }
        }
    }
}
