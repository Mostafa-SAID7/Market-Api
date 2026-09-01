using Market.Domain.Repositories;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Market.Infrastructure.Data;

namespace Market.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// User repository implementation for EF Core
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MarketDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Get all active users
        /// </summary>
        public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get users by role
        /// </summary>
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.Role == role && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(x => x.Email == email && !x.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Get users with vendor role
        /// </summary>
        public async Task<IEnumerable<User>> GetVendorsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.Role == UserRole.Vendor && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }
    }
}



