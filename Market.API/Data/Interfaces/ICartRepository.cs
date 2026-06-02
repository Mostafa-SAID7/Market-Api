using Market.API.Models.Entities;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Repository interface for cart-specific operations
    /// </summary>
    public interface ICartRepository : IRepository<Cart>
    {
        /// <summary>
        /// Get cart by user ID
        /// </summary>
        Task<Cart?> GetByUserIdAsync(string userId);

        /// <summary>
        /// Check if cart exists for user
        /// </summary>
        Task<bool> CartExistsAsync(string userId);
    }
}
