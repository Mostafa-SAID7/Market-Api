using Market.Domain.Repositories;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Review repository implementation for EF Core
    /// </summary>
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(MarketDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get reviews by product ID
        /// </summary>
        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get reviews by vendor ID
        /// </summary>
        public async Task<IEnumerable<Review>> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.VendorId == vendorId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get reviews by customer ID
        /// </summary>
        public async Task<IEnumerable<Review>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.CustomerId == customerId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get verified purchase reviews
        /// </summary>
        public async Task<IEnumerable<Review>> GetVerifiedReviewsAsync(int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId && x.IsVerifiedPurchase && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get reviews by rating
        /// </summary>
        public async Task<IEnumerable<Review>> GetByRatingAsync(int productId, int rating, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId && x.RatingValue == rating && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get top helpful reviews
        /// </summary>
        public async Task<IEnumerable<Review>> GetTopHelpfulAsync(int productId, int count = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId && !x.IsDeleted)
                .OrderByDescending(x => x.HelpfulCount)
                .ThenByDescending(x => x.CreatedAt)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Check if customer reviewed product
        /// </summary>
        public async Task<bool> CustomerReviewedProductAsync(int productId, int customerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(x => x.ProductId == productId && x.CustomerId == customerId && !x.IsDeleted, cancellationToken);
        }
    }
}

