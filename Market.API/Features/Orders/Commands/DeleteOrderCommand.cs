using MediatR;

namespace Market.API.Features.Orders.Commands
{
    /// <summary>
    /// Delete order command
    /// </summary>
    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Delete order command handler
    /// </summary>
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IMediator mediator, ILogger<DeleteOrderCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteOrderCommand for order: {OrderId}", request.Id);

            var result = await _mediator.Send(new DeleteOrderInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for deleting order
    /// </summary>
    internal class DeleteOrderInternalCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
