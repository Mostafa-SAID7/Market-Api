using Market.API.Models.Entities;
using Market.API.Models.Enums;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Repository interface for order-specific operations
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Get order by order number
        /// </summary>
        Task<Order?> GetByOrderNumberAsync(string orderNumber);

        /// <summary>
        /// Get orders by customer ID
        /// </summary>
        Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId);

        /// <summary>
        /// Get orders by status
        /// </summary>
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);

        /// <summary>
        /// Get orders by payment status
        /// </summary>
        Task<IEnumerable<Order>> GetByPaymentStatusAsync(PaymentStatus paymentStatus);

        /// <summary>
        /// Get pending orders
        /// </summary>
        Task<IEnumerable<Order>> GetPendingOrdersAsync();

        /// <summary>
        /// Get recent orders
        /// </summary>
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 50);
    }
}
