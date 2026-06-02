using Market.API.Models.Entities;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Repository interface for category-specific operations
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Get category by slug
        /// </summary>
        Task<Category?> GetBySlugAsync(string slug);

        /// <summary>
        /// Get all active categories
        /// </summary>
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();

        /// <summary>
        /// Get root categories (without parent)
        /// </summary>
        Task<IEnumerable<Category>> GetRootCategoriesAsync();

        /// <summary>
        /// Get subcategories by parent ID
        /// </summary>
        Task<IEnumerable<Category>> GetSubCategoriesAsync(string parentCategoryId);
    }
}
