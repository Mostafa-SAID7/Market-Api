using Market.API.Models.Entities;
using Market.API.Models.Enums;
using MongoDB.Driver;

namespace Market.API.Data.Seeds
{
    /// <summary>
    /// Seeds customer orders
    /// </summary>
    public class OrderSeeder
    {
        private readonly MongoDbContext _context;
        private readonly ILogger<OrderSeeder> _logger;

        public OrderSeeder(MongoDbContext context, ILogger<OrderSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                var existingOrders = await _context.Orders.CountDocumentsAsync(FilterDefinition<Order>.Empty);
                if (existingOrders > 0)
                {
                    _logger.LogInformation("Orders already exist. Skipping seeding.");
                    return;
                }

                var customer = await _context.Users.Find(u => u.Email == "customer1@market.com").FirstOrDefaultAsync();
                if (customer == null)
                {
                    _logger.LogWarning("Customer user not found");
                    return;
                }

                var vendor = await _context.Vendors.Find(v => v.StoreName == "Tech Paradise").FirstOrDefaultAsync();
                if (vendor == null)
                {
                    _logger.LogWarning("Vendor not found");
                    return;
                }

                var products = await _context.Products.Find(p => p.VendorId == vendor.Id).ToListAsync();
                if (products.Count == 0)
                {
                    _logger.LogWarning("Products not found");
                    return;
                }

                var orders = new List<Order>();

                // Order 1: Headphones and Stand
                var order1 = new Order
                {
                    CustomerId = customer.Id,
                    OrderNumber = Order.GenerateOrderNumber(),
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = products[0].Id, // Wireless Headphones
                            ProductName = products[0].Name,
                            VendorId = vendor.Id,
                            Price = products[0].Price,
                            Quantity = 1
                        },
                        new OrderItem
                        {
                            ProductId = products[1].Id, // Smartphone Stand
                            ProductName = products[1].Name,
                            VendorId = vendor.Id,
                            Price = products[1].Price,
                            Quantity = 2
                        }
                    },
                    ShippingCost = 10.00m,
                    Tax = 15.00m,
                    Status = OrderStatus.Delivered,
                    PaymentStatus = PaymentStatus.Completed,
                    ShippingAddress = "456 Main St, San Francisco, CA 94102",
                    TrackingNumber = "TRACK-2024-001",
                    Notes = "Please leave at front door"
                };
                order1.CalculateTotal();
                orders.Add(order1);

                // Order 2: USB-C Cable
                var order2 = new Order
                {
                    CustomerId = customer.Id,
                    OrderNumber = Order.GenerateOrderNumber(),
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductId = products[2].Id, // USB-C Cable
                            ProductName = products[2].Name,
                            VendorId = vendor.Id,
                            Price = products[2].Price,
                            Quantity = 3
                        }
                    },
                    ShippingCost = 5.00m,
                    Tax = 2.00m,
                    Status = OrderStatus.Delivered,
                    PaymentStatus = PaymentStatus.Completed,
                    ShippingAddress = "456 Main St, San Francisco, CA 94102",
                    TrackingNumber = "TRACK-2024-002"
                };
                order2.CalculateTotal();
                orders.Add(order2);

                await _context.Orders.InsertManyAsync(orders);
                _logger.LogInformation($"Seeded {orders.Count} orders");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding orders");
                throw;
            }
        }
    }
}
