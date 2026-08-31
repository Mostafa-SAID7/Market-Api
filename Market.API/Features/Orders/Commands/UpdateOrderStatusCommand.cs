using MediatR;
using Market.API.Models.Enums;

namespace Market.API.Features.Orders.Commands
{
    /// <summary>
    /// Update order status command
    /// </summary>
    public class UpdateOrderStatusCommand : IRequest<OrderResponse>
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }

    /// <summary>
    /// Update order status command handler
    /// </summary>
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, OrderResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateOrderStatusCommandHandler> _logger;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateOrderStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OrderResponse> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating order {OrderId} status to {Status}", request.Id, request.Status);

            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            order.OrderStatus = request.Status;
            await _unitOfWork.Orders.UpdateAsync(order);
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
