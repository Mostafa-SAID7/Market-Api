using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Repository for review operations
    /// </summary>
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetByProductIdAsync(string productId)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.ProductId, productId),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(r => r.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetByVendorIdAsync(string vendorId)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.VendorId, vendorId),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(r => r.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.CustomerId, customerId),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(r => r.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetVerifiedReviewsAsync(string productId)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.ProductId, productId),
                Builders<Review>.Filter.Eq(r => r.IsVerifiedPurchase, true),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(r => r.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetByRatingAsync(string productId, int rating)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.ProductId, productId),
                Builders<Review>.Filter.Eq(r => r.RatingValue, rating),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(r => r.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetTopHelpfulAsync(string productId, int count = 10)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.ProductId, productId),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            return await _collection
                .Find(filter)
                .SortByDescending(r => r.HelpfulCount)
                .ThenByDescending(r => r.CreatedAt)
                .Limit(count)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> CustomerReviewedProductAsync(string productId, string customerId)
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.ProductId, productId),
                Builders<Review>.Filter.Eq(r => r.CustomerId, customerId),
                Builders<Review>.Filter.Eq(r => r.IsDeleted, false)
            );
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
