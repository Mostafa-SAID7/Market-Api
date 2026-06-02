using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling cart business logic
    /// </summary>
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;

        public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            _logger.LogInformation("Fetching cart for user: {UserId}", userId);
            return await _unitOfWork.Carts.GetByUserIdAsync(userId);
        }

        /// <inheritdoc/>
        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            _logger.LogInformation("Getting or creating cart for user: {UserId}", userId);

            // Verify user exists
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            var existingCart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (existingCart != null)
            {
                _logger.LogInformation("Existing cart found for user: {UserId}", userId);
                return existingCart;
            }

            var newCart = new Cart { UserId = userId };
            await _unitOfWork.Carts.CreateAsync(newCart);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("New cart created for user: {UserId}", userId);
            return newCart;
        }

        /// <inheritdoc/>
        public async Task<Cart> AddItemToCartAsync(string userId, CartItem item)
        {
            _logger.LogInformation("Adding item to cart - User: {UserId}, Product: {ProductId}", userId, item.ProductId);

            if (string.IsNullOrWhiteSpace(item.ProductId))
                throw new ArgumentException("Product ID cannot be empty", nameof(item.ProductId));

            if (item.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(item.Quantity));

            var cart = await GetOrCreateCartAsync(userId);
            cart.AddItem(item);
            await _unitOfWork.Carts.UpdateAsync(cart.Id, cart);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Item added to cart - User: {UserId}", userId);
            return cart;
        }

        /// <inheritdoc/>
        public async Task<Cart> RemoveItemFromCartAsync(string userId, string productId)
        {
            _logger.LogInformation("Removing item from cart - User: {UserId}, Product: {ProductId}", userId, productId);

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                throw new KeyNotFoundException($"Cart not found for user {userId}");
            }

            cart.RemoveItem(productId);
            await _unitOfWork.Carts.UpdateAsync(cart.Id, cart);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Item removed from cart - User: {UserId}", userId);
            return cart;
        }

        /// <inheritdoc/>
        public async Task<Cart> UpdateItemQuantityAsync(string userId, string productId, int quantity)
        {
            _logger.LogInformation("Updating item quantity - User: {UserId}, Product: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                throw new KeyNotFoundException($"Cart not found for user {userId}");
            }

            cart.UpdateItemQuantity(productId, quantity);
            await _unitOfWork.Carts.UpdateAsync(cart.Id, cart);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Item quantity updated - User: {UserId}", userId);
            return cart;
        }

        /// <inheritdoc/>
        public async Task<Cart> ClearCartAsync(string userId)
        {
            _logger.LogInformation("Clearing cart - User: {UserId}", userId);

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                throw new KeyNotFoundException($"Cart not found for user {userId}");
            }

            cart.Clear();
            await _unitOfWork.Carts.UpdateAsync(cart.Id, cart);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Cart cleared - User: {UserId}", userId);
            return cart;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            _logger.LogInformation("Fetching cart items - User: {UserId}", userId);

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                return Enumerable.Empty<CartItem>();
            }

            return cart.Items;
        }

        /// <inheritdoc/>
        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            _logger.LogInformation("Calculating cart total - User: {UserId}", userId);

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                return 0;
            }

            return cart.SubTotal;
        }

        /// <inheritdoc/>
        public async Task<int> GetCartItemCountAsync(string userId)
        {
            _logger.LogInformation("Getting cart item count - User: {UserId}", userId);

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                return 0;
            }

            return cart.TotalItems;
        }

        /// <inheritdoc/>
        public async Task DeleteCartAsync(string userId)
        {
            _logger.LogInformation("Deleting cart - User: {UserId}", userId);

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user: {UserId}", userId);
                throw new KeyNotFoundException($"Cart not found for user {userId}");
            }

            await _unitOfWork.Carts.DeleteAsync(cart.Id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Cart deleted - User: {UserId}", userId);
        }
    }
}
