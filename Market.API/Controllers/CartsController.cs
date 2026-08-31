using MediatR;
using Market.API.Features.Carts.Commands;
using Market.API.Features.Carts.Queries;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CartsController> _logger;

        public CartsController(IMediator mediator, ILogger<CartsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get cart by user ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var query = new GetCartByUserIdQuery { UserId = userId };
            var cart = await _mediator.Send(query);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        /// <summary>
        /// Get or create cart for user
        /// </summary>
        [HttpGet("user/{userId}/get-or-create")]
        public async Task<IActionResult> GetOrCreate(int userId)
        {
            try
            {
                var query = new GetOrCreateCartQuery { UserId = userId };
                var cart = await _mediator.Send(query);
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
        public async Task<IActionResult> GetItems(int userId)
        {
            var query = new GetCartItemsQuery { UserId = userId };
            var items = await _mediator.Send(query);
            return Ok(items);
        }

        /// <summary>
        /// Get cart total
        /// </summary>
        [HttpGet("user/{userId}/total")]
        public async Task<IActionResult> GetTotal(int userId)
        {
            var query = new GetCartTotalQuery { UserId = userId };
            var total = await _mediator.Send(query);
            return Ok(new { total });
        }

        /// <summary>
        /// Get cart item count
        /// </summary>
        [HttpGet("user/{userId}/count")]
        public async Task<IActionResult> GetItemCount(int userId)
        {
            var query = new GetCartItemCountQuery { UserId = userId };
            var count = await _mediator.Send(query);
            return Ok(new { count });
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        [HttpPost("user/{userId}/items")]
        public async Task<IActionResult> AddItem(int userId, [FromBody] AddToCartCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.UserId = userId;

            try
            {
                var cart = await _mediator.Send(command);
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
        public async Task<IActionResult> RemoveItem(int userId, int productId)
        {
            try
            {
                var command = new RemoveFromCartCommand { UserId = userId, ProductId = productId };
                var cart = await _mediator.Send(command);
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
        public async Task<IActionResult> UpdateQuantity(int userId, int productId, [FromBody] UpdateCartItemQuantityCommand command)
        {
            try
            {
                command.UserId = userId;
                command.ProductId = productId;
                var cart = await _mediator.Send(command);
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
        public async Task<IActionResult> ClearCart(int userId)
        {
            try
            {
                var command = new ClearCartCommand { UserId = userId };
                var cart = await _mediator.Send(command);
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
        public async Task<IActionResult> DeleteCart(int userId)
        {
            try
            {
                var command = new DeleteCartCommand { UserId = userId };
                await _mediator.Send(command);
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
