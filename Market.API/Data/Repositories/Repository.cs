using Market.API.Common;
using Market.API.Data.Interfaces;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Generic repository implementation with MongoDB support
    /// </summary>
    /// <typeparam name="T">Entity type that extends BaseEntity</typeparam>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// Get all non-deleted entities
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection
                .Find(x => !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Get entity by ID (ignores soft-deleted flag)
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            return await _collection
                .Find(Builders<T>.Filter.Eq("_id", id))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Create new entity
        /// </summary>
        public virtual async Task CreateAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;
            await _collection.InsertOneAsync(entity);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        public virtual async Task UpdateAsync(string id, T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            await _collection.ReplaceOneAsync(
                Builders<T>.Filter.Eq("_id", id),
                entity
            );
        }

        /// <summary>
        /// Soft delete entity
        /// </summary>
        public virtual async Task DeleteAsync(string id)
        {
            var update = Builders<T>.Update
                .Set(x => x.IsDeleted, true)
                .Set(x => x.DeletedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(
                Builders<T>.Filter.Eq("_id", id),
                update
            );
        }
    }
}
