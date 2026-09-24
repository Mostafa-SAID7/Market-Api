using Market.Application.Abstractions.Services;
using Market.Application.Features.Products;
using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Persistence.Services
{
    /// <summary>
    /// Product read service implementation with EF Core SQL-side projections.
    /// Infrastructure-owned implementation that realizes the Application abstraction.
    /// This keeps EF Core references isolated to the Infrastructure layer.
    /// </summary>
    public class ProductReadService : IProductReadService
    {
        private readonly MarketDbContext _context;

        public ProductReadService(MarketDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(ProjectToResponse())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.CategoryId == categoryId && !x.IsDeleted)
                .Select(ProjectToResponse())
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(ProjectToResponse())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductResponse>> SearchProductsByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<ProductResponse>();

            return await _context.Products
                .AsNoTracking()
                .Where(x => x.Name.Contains(searchTerm) && !x.IsDeleted)
                .Select(ProjectToResponse())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && !x.IsDeleted)
                .Select(ProjectToResponse())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByVendorAsync(int vendorId, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.VendorId == vendorId && !x.IsDeleted)
                .Select(ProjectToResponse())
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// LINQ projection expression for Product → ProductResponse.
        /// Defined once, reused across all queries.
        /// SQL Server translates this to SELECT with only required columns.
        /// </summary>
        private static System.Linq.Expressions.Expression<Func<Product, ProductResponse>> ProjectToResponse()
        {
            return p => new ProductResponse
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
            };
        }
    }
}
