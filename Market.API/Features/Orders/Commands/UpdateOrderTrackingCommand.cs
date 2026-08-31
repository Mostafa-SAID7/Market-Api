using MediatR;

namespace Market.API.Features.Orders.Commands
{
    /// <summary>
    /// Update order tracking command
    /// </summary>
    public class UpdateOrderTrackingCommand : IRequest<OrderResponse>
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// Update order tracking command handler
    /// </summary>
    public class UpdateOrderTrackingCommandHandler : IRequestHandler<UpdateOrderTrackingCommand, OrderResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateOrderTrackingCommandHandler> _logger;

        public UpdateOrderTrackingCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateOrderTrackingCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OrderResponse> Handle(UpdateOrderTrackingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating order {OrderId} tracking number to {TrackingNumber}", request.Id, request.TrackingNumber);

            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            order.TrackingNumber = request.TrackingNumber;
            await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return MapToResponse(order);
        }

        private OrderResponse MapToResponse(Market.API.Models.Entities.Order order)
        {
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
                TrackingNumber = order.TrackingNumber,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}
