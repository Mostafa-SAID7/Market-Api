using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Product collection indexes
    /// </summary>
    public class ProductIndexes
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly ILogger<ProductIndexes> _logger;

        public ProductIndexes(IMongoCollection<Product> collection, ILogger<ProductIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // VendorId: filter products by vendor
                var productVendorIndex = new CreateIndexModel<Product>(
                    Builders<Product>.IndexKeys.Ascending(p => p.VendorId)
                );

                // Category: filter products by category
                var productCategoryIndex = new CreateIndexModel<Product>(
                    Builders<Product>.IndexKeys.Ascending(p => p.Category)
                );

                // IsDeleted: for soft-delete filtering
                var productNotDeletedIndex = new CreateIndexModel<Product>(
                    Builders<Product>.IndexKeys.Ascending(p => p.IsDeleted)
                );

                await _collection.Indexes.CreateOneAsync(productVendorIndex);
                await _collection.Indexes.CreateOneAsync(productCategoryIndex);
                await _collection.Indexes.CreateOneAsync(productNotDeletedIndex);
                _logger.LogDebug("Product indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("Product indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product indexes");
                throw;
            }
        }
    }
}
