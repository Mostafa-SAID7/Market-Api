using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartsController> _logger;

        public CartsController(ICartService cartService, ILogger<CartsController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Get cart by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        /// <summary>
        /// Get or create cart for user
        /// </summary>
        [HttpGet("user/{userId}/get-or-create")]
        public async Task<IActionResult> GetOrCreate(string userId)
        {
            try
            {
                var cart = await _cartService.GetOrCreateCartAsync(userId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get cart items
        /// </summary>
        [HttpGet("user/{userId}/items")]
        public async Task<IActionResult> GetItems(string userId)
        {
            var items = await _cartService.GetCartItemsAsync(userId);
            return Ok(items);
        }

        /// <summary>
        /// Get cart total
        /// </summary>
        [HttpGet("user/{userId}/total")]
        public async Task<IActionResult> GetTotal(string userId)
        {
            var total = await _cartService.GetCartTotalAsync(userId);
            return Ok(new { total });
        }

        /// <summary>
        /// Get cart item count
        /// </summary>
        [HttpGet("user/{userId}/count")]
        public async Task<IActionResult> GetItemCount(string userId)
        {
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(new { count });
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        [HttpPost("user/{userId}/items")]
        public async Task<IActionResult> AddItem(string userId, [FromBody] CartItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cart = await _cartService.AddItemToCartAsync(userId, item);
                return Ok(new { success = true, cart });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid item data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Resource not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        [HttpDelete("user/{userId}/items/{productId}")]
        public async Task<IActionResult> RemoveItem(string userId, string productId)
        {
            try
            {
                var cart = await _cartService.RemoveItemFromCartAsync(userId, productId);
                return Ok(new { success = true, cart });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Cart not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update item quantity
        /// </summary>
        [HttpPut("user/{userId}/items/{productId}/quantity")]
        public async Task<IActionResult> UpdateQuantity(string userId, string productId, [FromBody] dynamic request)
        {
            try
            {
                int quantity = request.quantity;
                var cart = await _cartService.UpdateItemQuantityAsync(userId, productId, quantity);
                return Ok(new { success = true, cart });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid quantity: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Cart not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Clear cart
        /// </summary>
        [HttpDelete("user/{userId}/clear")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            try
            {
                var cart = await _cartService.ClearCartAsync(userId);
                return Ok(new { success = true, cart });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Cart not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete cart
        /// </summary>
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteCart(string userId)
        {
            try
            {
                await _cartService.DeleteCartAsync(userId);
                return Ok(new { success = true, message = "Cart deleted" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Cart not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
