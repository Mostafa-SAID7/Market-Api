using Market.Application.Features.Products;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Product query repository interface for read-only DTO projections
    /// Keeps SQL-side filtering and projection while maintaining abstraction
    /// </summary>
    public interface IProductQueryRepository
    {
        /// <summary>
        /// Get all products as DTOs with SQL-side filtering and projection
        /// </summary>
        Task<IEnumerable<ProductResponse>> GetAllProductsAsResponseAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get products by category as DTOs with SQL-side filtering and projection
        /// </summary>
        Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsResponseAsync(int categoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get product by ID as DTO with SQL-side projection
        /// </summary>
        Task<ProductResponse?> GetProductByIdAsResponseAsync(int id, CancellationToken cancellationToken = default);
    }
}
