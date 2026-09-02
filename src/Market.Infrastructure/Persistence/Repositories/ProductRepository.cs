using Market.Domain.Repositories;
using Market.Domain.Entities;
using Market.Application.Features.Products;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Product repository implementation for EF Core
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository, IProductQueryRepository
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

        /// <summary>
        /// Get all products as DTOs with SQL-side filtering and projection
        /// Avoids loading entire entity graph into memory
        /// </summary>
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsResponseAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    ImageUrl = p.ImageUrl,
                    Quantity = p.Quantity,
                    Sold = p.Sold,
                    CategoryId = p.CategoryId,
                    VendorId = p.VendorId,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                })
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get products by category as DTOs with SQL-side filtering and projection
        /// Filters at database level, returns only required columns
        /// </summary>
        public async Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsResponseAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CategoryId == categoryId && !x.IsDeleted)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    ImageUrl = p.ImageUrl,
                    Quantity = p.Quantity,
                    Sold = p.Sold,
                    CategoryId = p.CategoryId,
                    VendorId = p.VendorId,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                })
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get product by ID as DTO with SQL-side projection
        /// </summary>
        public async Task<ProductResponse?> GetProductByIdAsResponseAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    ImageUrl = p.ImageUrl,
                    Quantity = p.Quantity,
                    Sold = p.Sold,
                    CategoryId = p.CategoryId,
                    VendorId = p.VendorId,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}



