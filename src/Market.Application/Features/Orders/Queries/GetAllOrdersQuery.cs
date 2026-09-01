using MediatR;
using Market.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Orders.Queries
{
    /// <summary>
    /// Get all orders query
    /// </summary>
    public class GetAllOrdersQuery : IRequest<List<OrderResponse>>
    {
    }

    /// <summary>
    /// Get all orders query handler
    /// </summary>
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllOrdersQueryHandler> _logger;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllOrdersQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllOrdersQuery");

            var orders = await _unitOfWork.Orders.GetAllAsync();
            return orders.Select(o => new OrderResponse
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderNumber = o.OrderNumber,
                Items = o.Items.Select(i => new OrderItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    VendorId = i.VendorId,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    SubTotal = i.SubTotal
                }).ToList(),
                SubTotal = o.SubTotal,
                ShippingCost = o.ShippingCost,
                Tax = o.Tax,
                TotalPrice = o.TotalPrice,
                OrderStatus = o.OrderStatus,
                PaymentStatus = o.PaymentStatus,
                ShippingAddress = o.ShippingAddress,
                TrackingNumber = o.TrackingNumber,
                Notes = o.Notes,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            }).ToList();
        }
    }
}



