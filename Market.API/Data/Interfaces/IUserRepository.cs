using Market.API.Models.Entities;
using Market.API.Models.Enums;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Repository interface for user-specific operations
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get user by email
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Get all active users
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync();

        /// <summary>
        /// Get users by role
        /// </summary>
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);

        /// <summary>
        /// Check if email exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Get vendors
        /// </summary>
        Task<IEnumerable<User>> GetVendorsAsync();
    }
}
