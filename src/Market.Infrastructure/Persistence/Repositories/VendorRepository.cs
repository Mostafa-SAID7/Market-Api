using Market.Domain.Repositories;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Vendor repository implementation for EF Core
    /// </summary>
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(MarketDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get vendor by user ID
        /// </summary>
        public async Task<Vendor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Get approved vendors
        /// </summary>
        public async Task<IEnumerable<Vendor>> GetApprovedVendorsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.IsApproved && x.IsActive && !x.IsDeleted)
                .OrderByDescending(x => x.AverageRating)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get active vendors
        /// </summary>
        public async Task<IEnumerable<Vendor>> GetActiveVendorsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get pending vendors (not approved)
        /// </summary>
        public async Task<IEnumerable<Vendor>> GetPendingVendorsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => !x.IsApproved && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get top rated vendors
        /// </summary>
        public async Task<IEnumerable<Vendor>> GetTopRatedVendorsAsync(int count = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.IsApproved && x.IsActive && !x.IsDeleted)
                .OrderByDescending(x => x.AverageRating)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}



