using MediatR;
using Market.API.Models.Entities;
using Market.API.Models.Enums;

namespace Market.API.Features.Orders.Commands
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
        private readonly IMediator _mediator;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IMediator mediator, ILogger<CreateOrderCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrderCommand for customer: {CustomerId}", request.CustomerId);

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderNumber = Order.GenerateOrderNumber(),
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    VendorId = i.VendorId,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                SubTotal = request.SubTotal,
                ShippingCost = request.ShippingCost,
                Tax = request.Tax,
                ShippingAddress = request.ShippingAddress,
                Notes = request.Notes,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending
            };

            order.CalculateTotal();

            var result = await _mediator.Send(new CreateOrderInternalCommand { Order = order }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for creating order
    /// </summary>
    internal class CreateOrderInternalCommand : IRequest<OrderResponse>
    {
        public Order Order { get; set; } = null!;
    }
}
