using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling category business logic
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Fetching all categories");
            return await _unitOfWork.Categories.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Category?> GetCategoryByIdAsync(string id)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
            return await _unitOfWork.Categories.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            _logger.LogInformation("Fetching category with slug: {Slug}", slug);
            return await _unitOfWork.Categories.GetBySlugAsync(slug);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            _logger.LogInformation("Fetching all active categories");
            return await _unitOfWork.Categories.GetActiveCategoriesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            _logger.LogInformation("Fetching root categories");
            return await _unitOfWork.Categories.GetRootCategoriesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(string parentCategoryId)
        {
            _logger.LogInformation("Fetching subcategories for parent: {ParentCategoryId}", parentCategoryId);
            
            var parent = await _unitOfWork.Categories.GetByIdAsync(parentCategoryId);
            if (parent == null)
            {
                _logger.LogWarning("Parent category not found: {ParentCategoryId}", parentCategoryId);
                return Enumerable.Empty<Category>();
            }

            return await _unitOfWork.Categories.GetSubCategoriesAsync(parentCategoryId);
        }

        /// <inheritdoc/>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _logger.LogInformation("Creating new category: {CategoryName}", category.Name);

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be empty", nameof(category.Name));

            // Check for duplicate slug
            var existingCategory = await _unitOfWork.Categories.GetBySlugAsync(category.SlugValue);
            if (existingCategory != null)
            {
                _logger.LogWarning("Category with slug already exists: {Slug}", category.SlugValue);
                throw new InvalidOperationException($"Category with slug '{category.SlugValue}' already exists");
            }

            await _unitOfWork.Categories.CreateAsync(category);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Category created successfully with ID: {CategoryId}", category.Id);
            return category;
        }

        /// <inheritdoc/>
        public async Task<Category> UpdateCategoryAsync(string id, Category category)
        {
            _logger.LogInformation("Updating category with ID: {CategoryId}", id);

            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
            {
                _logger.LogWarning("Category not found for update: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            // Check if slug changed and if new slug is unique
            if (existingCategory.SlugValue != category.SlugValue)
            {
                var categoryWithSlug = await _unitOfWork.Categories.GetBySlugAsync(category.SlugValue);
                if (categoryWithSlug != null && categoryWithSlug.Id != id)
                {
                    _logger.LogWarning("Category with slug already exists: {Slug}", category.SlugValue);
                    throw new InvalidOperationException($"Category with slug '{category.SlugValue}' already exists");
                }
            }

            category.Id = id;
            await _unitOfWork.Categories.UpdateAsync(id, category);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Category updated successfully: {CategoryId}", id);
            return category;
        }

        /// <inheritdoc/>
        public async Task DeleteCategoryAsync(string id)
        {
            _logger.LogInformation("Deleting category with ID: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category not found for deletion: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            // Check if category has products
            var allProducts = await _unitOfWork.Products.GetAllAsync();
            if (allProducts.Any(p => p.Category == id))
            {
                _logger.LogWarning("Cannot delete category with products: {CategoryId}", id);
                throw new InvalidOperationException("Cannot delete category that has associated products");
            }

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Category deleted successfully: {CategoryId}", id);
        }

        /// <inheritdoc/>
        public async Task<Category> SetCategoryActiveStatusAsync(string id, bool isActive)
        {
            _logger.LogInformation("Setting category active status - ID: {CategoryId}, IsActive: {IsActive}", id, isActive);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category not found: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            category.IsActive = isActive;
            await _unitOfWork.Categories.UpdateAsync(id, category);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Category status updated: {CategoryId}", id);
            return category;
        }
    }
}
