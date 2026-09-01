using Market.Domain.Repositories;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Category repository implementation for EF Core
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MarketDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get category by slug
        /// </summary>
        public async Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Slug == slug && !x.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Get all active categories
        /// </summary>
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get root categories (without parent)
        /// </summary>
        public async Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.ParentCategoryId == null && !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get subcategories by parent ID
        /// </summary>
        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.ParentCategoryId == parentCategoryId && !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken);
        }
    }
}



