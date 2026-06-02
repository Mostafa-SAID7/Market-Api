using Market.API.Data.Interfaces;
using Market.API.Models.Entities;
using Market.API.Models.Enums;
using Market.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Market.API.Data.Repositories
{
    /// <summary>
    /// Repository for order operations
    /// </summary>
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        /// <inheritdoc/>
        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderNumber, orderNumber);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(o => o.CustomerId, customerId),
                Builders<Order>.Filter.Eq(o => o.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(o => o.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(o => o.Status, status),
                Builders<Order>.Filter.Eq(o => o.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(o => o.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetByPaymentStatusAsync(PaymentStatus paymentStatus)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(o => o.PaymentStatus, paymentStatus),
                Builders<Order>.Filter.Eq(o => o.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(o => o.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Eq(o => o.Status, OrderStatus.Pending),
                Builders<Order>.Filter.Eq(o => o.IsDeleted, false)
            );
            return await _collection.Find(filter).SortByDescending(o => o.CreatedAt).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 50)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.IsDeleted, false);
            return await _collection
                .Find(filter)
                .SortByDescending(o => o.CreatedAt)
                .Limit(count)
                .ToListAsync();
        }
    }
}
