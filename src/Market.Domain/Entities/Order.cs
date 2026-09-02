using Market.Domain.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities
{
    /// <summary>
    /// Order entity for e-commerce platform
    /// </summary>
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string ShippingAddress { get; set; } = string.Empty;
        public string? TrackingNumber { get; set; }

        public string? Notes { get; set; }

        // Navigation properties
        public User Customer { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

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
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;

        // Navigation properties
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Vendor Vendor { get; set; } = null!;
    }
}

