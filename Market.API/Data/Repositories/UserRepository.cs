using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Models.Enums;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Repository for user operations
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <inheritdoc/>
        public async Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.IsActive, true),
                Builders<User>.Filter.Eq(u => u.IsDeleted, false)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Role, role),
                Builders<User>.Filter.Eq(u => u.IsDeleted, false)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> EmailExistsAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetVendorsAsync()
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Role, UserRole.Vendor),
                Builders<User>.Filter.Eq(u => u.IsDeleted, false)
            );
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
