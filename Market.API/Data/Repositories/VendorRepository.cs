using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Repository for vendor operations
    /// </summary>
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <inheritdoc/>
        public async Task<Vendor?> GetByUserIdAsync(string userId)
        {
            var filter = Builders<Vendor>.Filter.Eq(v => v.UserId, userId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetApprovedVendorsAsync()
        {
            var filter = Builders<Vendor>.Filter.And(
                Builders<Vendor>.Filter.Eq(v => v.IsApproved, true),
                Builders<Vendor>.Filter.Eq(v => v.IsDeleted, false)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetActiveVendorsAsync()
        {
            var filter = Builders<Vendor>.Filter.And(
                Builders<Vendor>.Filter.Eq(v => v.IsActive, true),
                Builders<Vendor>.Filter.Eq(v => v.IsApproved, true),
                Builders<Vendor>.Filter.Eq(v => v.IsDeleted, false)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetPendingVendorsAsync()
        {
            var filter = Builders<Vendor>.Filter.And(
                Builders<Vendor>.Filter.Eq(v => v.IsApproved, false),
                Builders<Vendor>.Filter.Eq(v => v.IsDeleted, false)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10)
        {
            var filter = Builders<Vendor>.Filter.And(
                Builders<Vendor>.Filter.Eq(v => v.IsActive, true),
                Builders<Vendor>.Filter.Eq(v => v.IsApproved, true),
                Builders<Vendor>.Filter.Eq(v => v.IsDeleted, false)
            );

            return await _collection
                .Find(filter)
                .SortByDescending(v => v.AverageRating)
                .Limit(count)
                .ToListAsync();
        }
    }
}
