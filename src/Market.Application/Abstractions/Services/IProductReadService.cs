using Market.Application.Features.Products;

namespace Market.Application.Abstractions.Services
{
    /// <summary>
    /// Product read service abstraction for SQL-side query and projection operations.
    /// Owned by Application layer. Infrastructure implements this contract.
    /// This abstraction ensures Infrastructure does not depend on Application.Features.
    /// </summary>
    public interface IProductReadService
    {
        /// <summary>
        /// Get all active products as response DTOs with SQL-side filtering and projection.
        /// Only required columns are selected at database level.
        /// </summary>
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get products by category ID as response DTOs with SQL-side filtering.
        /// Filters and projects at database level for efficiency.
        /// </summary>
        Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get product by ID as response DTO with SQL-side projection.
        /// Returns null if product not found or is deleted.
        /// </summary>
        Task<ProductResponse?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search products by name with SQL-side filtering and projection.
        /// Returns only active products matching the search term.
        /// </summary>
        Task<IEnumerable<ProductResponse>> SearchProductsByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get products by price range with SQL-side filtering.
        /// Filters and projects at database level.
        /// </summary>
        Task<IEnumerable<ProductResponse>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get products by vendor ID as response DTOs.
        /// SQL-side filtering and projection.
        /// </summary>
        Task<IEnumerable<ProductResponse>> GetProductsByVendorAsync(int vendorId, CancellationToken cancellationToken = default);
    }
}
