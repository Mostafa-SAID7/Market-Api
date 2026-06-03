using Market.API.Models.Entities;
using MongoDB.Driver;

namespace Market.API.Data.Indexes
{
    /// <summary>
    /// Vendor collection indexes
    /// </summary>
    public class VendorIndexes
    {
        private readonly IMongoCollection<Vendor> _collection;
        private readonly ILogger<VendorIndexes> _logger;

        public VendorIndexes(IMongoCollection<Vendor> collection, ILogger<VendorIndexes> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            try
            {
                // Currently no specific indexes defined for Vendor
                // Add indexes here as needed for vendor queries
                _logger.LogDebug("Vendor indexes created successfully");
            }
            catch (MongoDB.Driver.MongoCommandException ex) when (ex.Message.Contains("already has an index") || ex.Message.Contains("existing index has the same name"))
            {
                _logger.LogDebug("Vendor indexes already exist - skipping creation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vendor indexes");
                throw;
            }
        }
    }
}
