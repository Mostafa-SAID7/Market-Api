using Market.API.Models.Entities;
using Market.API.Models.Enums;
using Market.API.Services.Interfaces;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Get order by order number
        /// </summary>
        [HttpGet("number/{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Get orders by customer ID
        /// </summary>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }

        /// <summary>
        /// Get orders by status
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        /// <summary>
        /// Get orders by payment status
        /// </summary>
        [HttpGet("payment-status/{paymentStatus}")]
        public async Task<IActionResult> GetByPaymentStatus(PaymentStatus paymentStatus)
        {
            var orders = await _orderService.GetOrdersByPaymentStatusAsync(paymentStatus);
            return Ok(orders);
        }

        /// <summary>
        /// Get pending orders
        /// </summary>
        [HttpGet("pending/list")]
        public async Task<IActionResult> GetPending()
        {
            var orders = await _orderService.GetPendingOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Get recent orders
        /// </summary>
        [HttpGet("recent/list")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 50)
        {
            var orders = await _orderService.GetRecentOrdersAsync(count);
            return Ok(orders);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(Get), new { id = createdOrder.Id }, createdOrder);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid order data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Resource not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing order
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, order);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            try
            {
                var order = await _orderService.CancelOrderAsync(id);
                return Ok(new { success = true, order });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update order status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] dynamic request)
        {
            try
            {
                OrderStatus status = request.status;
                var order = await _orderService.UpdateOrderStatusAsync(id, status);
                return Ok(new { success = true, order });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update payment status
        /// </summary>
        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(string id, [FromBody] dynamic request)
        {
            try
            {
                PaymentStatus paymentStatus = request.paymentStatus;
                var order = await _orderService.UpdatePaymentStatusAsync(id, paymentStatus);
                return Ok(new { success = true, order });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update tracking number
        /// </summary>
        [HttpPut("{id}/tracking")]
        public async Task<IActionResult> UpdateTracking(string id, [FromBody] dynamic request)
        {
            try
            {
                string trackingNumber = request.trackingNumber;
                var order = await _orderService.UpdateTrackingNumberAsync(id, trackingNumber);
                return Ok(new { success = true, order });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an order
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return Ok(new { success = true, message = "Order deleted" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
