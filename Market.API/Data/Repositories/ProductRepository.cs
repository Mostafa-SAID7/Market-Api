using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Product-specific repository with custom queries
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <summary>
        /// Get products by price range (excluding soft-deleted)
        /// </summary>
        public async Task<IEnumerable<Product>> GetByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _collection
                .Find(p => p.Price >= minPrice && p.Price <= maxPrice && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
