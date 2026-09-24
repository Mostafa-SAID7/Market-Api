using MediatR;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Orders.Commands
{
    /// <summary>
    /// Create order command
    /// </summary>
    public class CreateOrderCommand : IRequest<OrderResponse>
    {
        public int CustomerId { get; set; }
        public List<OrderItemInput> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Tax { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Order item input for creating order
    /// </summary>
    public class OrderItemInput
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Create order command handler
    /// </summary>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrderCommand for customer: {CustomerId}", request.CustomerId);

            if (request.CustomerId <= 0 || !await _unitOfWork.Users.ExistsAsync(request.CustomerId, cancellationToken))
                throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");
            if (request.Items.Count == 0)
                throw new ArgumentException("Order must contain at least one item.", nameof(request.Items));
            if (string.IsNullOrWhiteSpace(request.ShippingAddress) || request.ShippingAddress.Length < 10)
                throw new ArgumentException("Shipping address must be at least 10 characters.", nameof(request.ShippingAddress));
            if (request.ShippingCost < 0 || request.Tax < 0)
                throw new ArgumentException("Shipping cost and tax cannot be negative.");

            var items = new List<OrderItem>();
            foreach (var input in request.Items)
            {
                if (input.ProductId <= 0 || input.Quantity <= 0)
                    throw new ArgumentException("Each order item must have a valid product ID and positive quantity.");

                var product = await _unitOfWork.Products.GetByIdAsync(input.ProductId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Product with ID {input.ProductId} not found");
                if (!product.IsInStock || product.Quantity < input.Quantity)
                    throw new InvalidOperationException($"Product with ID {input.ProductId} does not have sufficient stock.");

                product.Quantity -= input.Quantity;
                product.Sold += input.Quantity;
                await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
                items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    VendorId = product.VendorId,
                    Price = product.DiscountPrice ?? product.Price,
                    Quantity = input.Quantity
                });
            }

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderNumber = Order.GenerateOrderNumber(),
                Items = items,
                SubTotal = 0,
                ShippingCost = request.ShippingCost,
                Tax = request.Tax,
                ShippingAddress = request.ShippingAddress,
                Notes = request.Notes,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending
            };

            order.CalculateTotal();

            await _unitOfWork.Orders.CreateAsync(order, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderNumber = order.OrderNumber,
                SubTotal = order.SubTotal,
                ShippingCost = order.ShippingCost,
                Tax = order.Tax,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(item => new OrderItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    VendorId = item.VendorId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    SubTotal = item.SubTotal
                }).ToList()
            };
        }
    }
}


