using Market.API.Common;
using Market.API.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// Order entity for e-commerce platform
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Order : BaseEntity
    {
        public string CustomerId { get; set; } = string.Empty; // Reference to User
        public string OrderNumber { get; set; } = string.Empty; // Unique order number
        
        public List<OrderItem> Items { get; set; } = new();
        
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }

        public string? Notes { get; set; }

        /// <summary>
        /// Generate unique order number
        /// </summary>
        public static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        /// <summary>
        /// Calculate total price
        /// </summary>
        public void CalculateTotal()
        {
            SubTotal = Items.Sum(x => x.SubTotal);
            TotalPrice = SubTotal + ShippingCost + Tax;
        }
    }

    /// <summary>
    /// Order item - product in an order
    /// </summary>
    public class OrderItem
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;
    }
}
