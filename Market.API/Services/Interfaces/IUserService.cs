using Market.API.Models.Entities;
using Market.API.Models.Enums;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for user operations
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Get user by ID
        /// </summary>
        Task<User?> GetUserByIdAsync(string id);

        /// <summary>
        /// Get user by email
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get active users
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync();

        /// <summary>
        /// Get users by role
        /// </summary>
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);

        /// <summary>
        /// Get vendors
        /// </summary>
        Task<IEnumerable<User>> GetVendorsAsync();

        /// <summary>
        /// Create a new user
        /// </summary>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Update an existing user
        /// </summary>
        Task<User> UpdateUserAsync(string id, User user);

        /// <summary>
        /// Delete a user
        /// </summary>
        Task DeleteUserAsync(string id);

        /// <summary>
        /// Verify user email
        /// </summary>
        Task<User> VerifyEmailAsync(string id);

        /// <summary>
        /// Set user active status
        /// </summary>
        Task<User> SetUserActiveStatusAsync(string id, bool isActive);
    }
}
