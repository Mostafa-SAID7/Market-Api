using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Models.Enums;
using Market.API.Services.Interfaces;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling order business logic
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Fetching all orders");
            return await _unitOfWork.Orders.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            _logger.LogInformation("Fetching order with ID: {OrderId}", id);
            return await _unitOfWork.Orders.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            _logger.LogInformation("Fetching order with number: {OrderNumber}", orderNumber);
            return await _unitOfWork.Orders.GetByOrderNumberAsync(orderNumber);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId)
        {
            _logger.LogInformation("Fetching orders for customer: {CustomerId}", customerId);
            
            var customer = await _unitOfWork.Users.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", customerId);
                return Enumerable.Empty<Order>();
            }

            return await _unitOfWork.Orders.GetByCustomerIdAsync(customerId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            _logger.LogInformation("Fetching orders with status: {Status}", status);
            return await _unitOfWork.Orders.GetByStatusAsync(status);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetOrdersByPaymentStatusAsync(PaymentStatus paymentStatus)
        {
            _logger.LogInformation("Fetching orders with payment status: {PaymentStatus}", paymentStatus);
            return await _unitOfWork.Orders.GetByPaymentStatusAsync(paymentStatus);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            _logger.LogInformation("Fetching pending orders");
            return await _unitOfWork.Orders.GetPendingOrdersAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 50)
        {
            _logger.LogInformation("Fetching recent {Count} orders", count);
            return await _unitOfWork.Orders.GetRecentOrdersAsync(count);
        }

        /// <inheritdoc/>
        public async Task<Order> CreateOrderAsync(Order order)
        {
            _logger.LogInformation("Creating new order for customer: {CustomerId}", order.CustomerId);

            if (string.IsNullOrWhiteSpace(order.CustomerId))
                throw new ArgumentException("Customer ID cannot be empty", nameof(order.CustomerId));

            if (order.Items == null || order.Items.Count == 0)
                throw new ArgumentException("Order must have at least one item", nameof(order.Items));

            // Verify customer exists
            var customer = await _unitOfWork.Users.GetByIdAsync(order.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", order.CustomerId);
                throw new KeyNotFoundException($"Customer with ID {order.CustomerId} not found");
            }

            // Generate order number if not provided
            if (string.IsNullOrWhiteSpace(order.OrderNumber))
                order.OrderNumber = Order.GenerateOrderNumber();

            // Calculate total
            order.CalculateTotal();

            await _unitOfWork.Orders.CreateAsync(order);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Order created successfully with ID: {OrderId}, Order Number: {OrderNumber}", order.Id, order.OrderNumber);
            return order;
        }

        /// <inheritdoc/>
        public async Task<Order> UpdateOrderAsync(string id, Order order)
        {
            _logger.LogInformation("Updating order with ID: {OrderId}", id);

            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(id);
            if (existingOrder == null)
            {
                _logger.LogWarning("Order not found for update: {OrderId}", id);
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            order.Id = id;
            order.OrderNumber = existingOrder.OrderNumber; // Prevent order number change
            order.CalculateTotal();
            await _unitOfWork.Orders.UpdateAsync(id, order);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Order updated successfully: {OrderId}", id);
            return order;
        }

        /// <inheritdoc/>
        public async Task<Order> CancelOrderAsync(string id)
        {
            _logger.LogInformation("Cancelling order with ID: {OrderId}", id);

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered)
            {
                _logger.LogWarning("Cannot cancel shipped/delivered order: {OrderId}", id);
                throw new InvalidOperationException("Cannot cancel a shipped or delivered order");
            }

            order.Status = OrderStatus.Cancelled;
            await _unitOfWork.Orders.UpdateAsync(id, order);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Order cancelled successfully: {OrderId}", id);
            return order;
        }

        /// <inheritdoc/>
        public async Task<Order> UpdateOrderStatusAsync(string id, OrderStatus status)
        {
            _logger.LogInformation("Updating order status - ID: {OrderId}, Status: {Status}", id, status);

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            order.Status = status;
            await _unitOfWork.Orders.UpdateAsync(id, order);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Order status updated: {OrderId}", id);
            return order;
        }

        /// <inheritdoc/>
        public async Task<Order> UpdatePaymentStatusAsync(string id, PaymentStatus paymentStatus)
        {
            _logger.LogInformation("Updating payment status - ID: {OrderId}, PaymentStatus: {PaymentStatus}", id, paymentStatus);

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            order.PaymentStatus = paymentStatus;
            await _unitOfWork.Orders.UpdateAsync(id, order);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Payment status updated: {OrderId}", id);
            return order;
        }

        /// <inheritdoc/>
        public async Task<Order> UpdateTrackingNumberAsync(string id, string trackingNumber)
        {
            _logger.LogInformation("Updating tracking number - ID: {OrderId}, Tracking: {TrackingNumber}", id, trackingNumber);

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            order.TrackingNumber = trackingNumber;
            await _unitOfWork.Orders.UpdateAsync(id, order);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Tracking number updated: {OrderId}", id);
            return order;
        }

        /// <inheritdoc/>
        public async Task DeleteOrderAsync(string id)
        {
            _logger.LogInformation("Deleting order with ID: {OrderId}", id);

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found for deletion: {OrderId}", id);
                throw new KeyNotFoundException($"Order with ID {id} not found");
            }

            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Order deleted successfully: {OrderId}", id);
        }
    }
}
