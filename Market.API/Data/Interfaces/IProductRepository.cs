using Market.API.Models.Entities;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Product-specific repository interface
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get products by price range
        /// </summary>
        Task<IEnumerable<Product>> GetByPriceRange(decimal minPrice, decimal maxPrice);
    }
}
