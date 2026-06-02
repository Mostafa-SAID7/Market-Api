using MediatR;
using Market.API.Models.Enums;

namespace Market.API.Features.Orders.Commands
{
    /// <summary>
    /// Update order command
    /// </summary>
    public class UpdateOrderCommand : IRequest<OrderResponse>
    {
        public string Id { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Update order command handler
    /// </summary>
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IMediator mediator, ILogger<UpdateOrderCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<OrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateOrderCommand for order: {OrderId}", request.Id);

            var result = await _mediator.Send(
                new UpdateOrderInternalCommand
                {
                    Id = request.Id,
                    Status = request.Status,
                    PaymentStatus = request.PaymentStatus,
                    TrackingNumber = request.TrackingNumber,
                    Notes = request.Notes
                },
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for updating order
    /// </summary>
    internal class UpdateOrderInternalCommand : IRequest<OrderResponse>
    {
        public string Id { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
    }
}
