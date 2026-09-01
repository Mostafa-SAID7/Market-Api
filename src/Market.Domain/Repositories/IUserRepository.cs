using Market.Domain.Entities;
using Market.Domain.Enums;

namespace Market.Domain.Repositories
{
    /// <summary>
    /// Repository interface for user-specific operations
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get user by email
        /// </summary>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all active users
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get users by role
        /// </summary>
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if email exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get users with vendor role
        /// </summary>
        Task<IEnumerable<User>> GetVendorsAsync(CancellationToken cancellationToken = default);
    }
}


