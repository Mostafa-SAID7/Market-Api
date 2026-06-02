using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Repository for cart operations
    /// </summary>
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <inheritdoc/>
        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId, userId),
                Builders<Cart>.Filter.Eq(c => c.IsDeleted, false)
            );
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> CartExistsAsync(string userId)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId, userId),
                Builders<Cart>.Filter.Eq(c => c.IsDeleted, false)
            );
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
