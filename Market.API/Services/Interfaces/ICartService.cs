using Market.API.Models.Entities;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for cart operations
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Get cart by user ID
        /// </summary>
        Task<Cart?> GetCartByUserIdAsync(string userId);

        /// <summary>
        /// Get or create cart for user
        /// </summary>
        Task<Cart> GetOrCreateCartAsync(string userId);

        /// <summary>
        /// Add item to cart
        /// </summary>
        Task<Cart> AddItemToCartAsync(string userId, CartItem item);

        /// <summary>
        /// Remove item from cart
        /// </summary>
        Task<Cart> RemoveItemFromCartAsync(string userId, string productId);

        /// <summary>
        /// Update item quantity in cart
        /// </summary>
        Task<Cart> UpdateItemQuantityAsync(string userId, string productId, int quantity);

        /// <summary>
        /// Clear cart
        /// </summary>
        Task<Cart> ClearCartAsync(string userId);

        /// <summary>
        /// Get all items in cart
        /// </summary>
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);

        /// <summary>
        /// Get cart total
        /// </summary>
        Task<decimal> GetCartTotalAsync(string userId);

        /// <summary>
        /// Get cart item count
        /// </summary>
        Task<int> GetCartItemCountAsync(string userId);

        /// <summary>
        /// Delete cart
        /// </summary>
        Task DeleteCartAsync(string userId);
    }
}
