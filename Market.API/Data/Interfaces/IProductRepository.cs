using Market.API.Data.Repositories;
using Market.API.Models.Entities;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Product repository interface with specialized queries
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get products by category ID
        /// </summary>
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get products by vendor ID
        /// </summary>
        Task<IEnumerable<Product>> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get products by price range
        /// </summary>
        Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search products by name
        /// </summary>
        Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get product by SKU
        /// </summary>
        Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    }
}
