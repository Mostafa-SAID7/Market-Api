using Market.API.Models.Entities;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for category operations
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Get category by ID
        /// </summary>
        Task<Category?> GetCategoryByIdAsync(string id);

        /// <summary>
        /// Get category by slug
        /// </summary>
        Task<Category?> GetCategoryBySlugAsync(string slug);

        /// <summary>
        /// Get active categories
        /// </summary>
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();

        /// <summary>
        /// Get root categories (no parent)
        /// </summary>
        Task<IEnumerable<Category>> GetRootCategoriesAsync();

        /// <summary>
        /// Get subcategories by parent ID
        /// </summary>
        Task<IEnumerable<Category>> GetSubCategoriesAsync(string parentCategoryId);

        /// <summary>
        /// Create a new category
        /// </summary>
        Task<Category> CreateCategoryAsync(Category category);

        /// <summary>
        /// Update an existing category
        /// </summary>
        Task<Category> UpdateCategoryAsync(string id, Category category);

        /// <summary>
        /// Delete a category
        /// </summary>
        Task DeleteCategoryAsync(string id);

        /// <summary>
        /// Activate/Deactivate category
        /// </summary>
        Task<Category> SetCategoryActiveStatusAsync(string id, bool isActive);
    }
}
