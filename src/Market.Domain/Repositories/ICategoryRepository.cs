using Market.Domain.Entities;

namespace Market.Domain.Repositories
{
    /// <summary>
    /// Repository interface for category-specific operations
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Get category by slug
        /// </summary>
        Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all active categories
        /// </summary>
        Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get root categories (without parent)
        /// </summary>
        Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get subcategories by parent ID
        /// </summary>
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default);
    }
}


