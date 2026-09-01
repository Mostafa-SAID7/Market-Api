using Market.Domain.Repositories;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Cart repository implementation for EF Core
    /// </summary>
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(MarketDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get cart by user ID
        /// </summary>
        public async Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Check if cart exists for user
        /// </summary>
        public async Task<bool> CartExistsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(x => x.UserId == userId && !x.IsDeleted, cancellationToken);
        }
    }
}



