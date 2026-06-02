using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Repository for category operations
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <inheritdoc/>
        public async Task<Category?> GetBySlugAsync(string slug)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.SlugValue, slug);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            var filter = Builders<Category>.Filter.Eq(c => c.IsActive, true);
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            var filter = Builders<Category>.Filter.Eq(c => c.ParentCategoryId, null);
            return await _collection.Find(filter).SortBy(c => c.DisplayOrder).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(string parentCategoryId)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.ParentCategoryId, parentCategoryId);
            return await _collection.Find(filter).SortBy(c => c.DisplayOrder).ToListAsync();
        }
    }
}
