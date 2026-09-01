using Market.Domain.Entities;

namespace Market.Domain.Repositories
{
    /// <summary>
    /// Repository interface for cart-specific operations
    /// </summary>
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// Get cart by user ID
        /// </summary>
        Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if cart exists for user
        /// </summary>
        Task<bool> CartExistsAsync(int userId, CancellationToken cancellationToken = default);
    }
}


