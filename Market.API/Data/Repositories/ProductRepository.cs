using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Product repository implementation for EF Core
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MarketDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get products by category ID
        /// </summary>
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.CategoryId == categoryId && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get products by vendor ID
        /// </summary>
        public async Task<IEnumerable<Product>> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.VendorId == vendorId && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get products by price range
        /// </summary>
        public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Search products by name
        /// </summary>
        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.Name.Contains(searchTerm) && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get product by SKU
        /// </summary>
        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.SKU == sku && !x.IsDeleted, cancellationToken);
        }
    }
}
