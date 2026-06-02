using Market.API.Models.Entities;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for product operations
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get all products
        /// </summary>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// Get product by ID
        /// </summary>
        Task<Product?> GetProductByIdAsync(string id);

        /// <summary>
        /// Get products by price range
        /// </summary>
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        /// <summary>
        /// Create a new product
        /// </summary>
        Task<Product> CreateProductAsync(Product product);

        /// <summary>
        /// Update an existing product
        /// </summary>
        Task<Product> UpdateProductAsync(string id, Product product);

        /// <summary>
        /// Delete a product
        /// </summary>
        Task DeleteProductAsync(string id);
    }
}
