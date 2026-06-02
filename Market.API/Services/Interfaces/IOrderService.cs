using Market.API.Models.Entities;
using Market.API.Models.Enums;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for order operations
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Get all orders
        /// </summary>
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        /// <summary>
        /// Get order by ID
        /// </summary>
        Task<Order?> GetOrderByIdAsync(string id);

        /// <summary>
        /// Get order by order number
        /// </summary>
        Task<Order?> GetOrderByNumberAsync(string orderNumber);

        /// <summary>
        /// Get orders by customer
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);

        /// <summary>
        /// Get orders by status
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);

        /// <summary>
        /// Get orders by payment status
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByPaymentStatusAsync(PaymentStatus paymentStatus);

        /// <summary>
        /// Get pending orders
        /// </summary>
        Task<IEnumerable<Order>> GetPendingOrdersAsync();

        /// <summary>
        /// Get recent orders
        /// </summary>
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 50);

        /// <summary>
        /// Create a new order
        /// </summary>
        Task<Order> CreateOrderAsync(Order order);

        /// <summary>
        /// Update an existing order
        /// </summary>
        Task<Order> UpdateOrderAsync(string id, Order order);

        /// <summary>
        /// Cancel an order
        /// </summary>
        Task<Order> CancelOrderAsync(string id);

        /// <summary>
        /// Update order status
        /// </summary>
        Task<Order> UpdateOrderStatusAsync(string id, OrderStatus status);

        /// <summary>
        /// Update payment status
        /// </summary>
        Task<Order> UpdatePaymentStatusAsync(string id, PaymentStatus paymentStatus);

        /// <summary>
        /// Update tracking number
        /// </summary>
        Task<Order> UpdateTrackingNumberAsync(string id, string trackingNumber);

        /// <summary>
        /// Delete an order
        /// </summary>
        Task DeleteOrderAsync(string id);
    }
}
