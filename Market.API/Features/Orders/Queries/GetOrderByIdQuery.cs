using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Orders.Queries
{
    /// <summary>
    /// Get order by id query
    /// </summary>
    public class GetOrderByIdQuery : IRequest<OrderResponse?>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Get order by id query handler
    /// </summary>
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetOrderByIdQueryHandler> _logger;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetOrderByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OrderResponse?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetOrderByIdQuery for order: {OrderId}", request.Id);

            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
            if (order == null)
                return null;

            return new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderNumber = order.OrderNumber,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    VendorId = i.VendorId,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    SubTotal = i.SubTotal
                }).ToList(),
                SubTotal = order.SubTotal,
                ShippingCost = order.ShippingCost,
                Tax = order.Tax,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                ShippingAddress = order.ShippingAddress,
                TrackingNumber = order.TrackingNumber,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}
