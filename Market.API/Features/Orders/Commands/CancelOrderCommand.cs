using MediatR;

namespace Market.API.Features.Orders.Commands
{
    public class CancelOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Reason { get; set; }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(ILogger<CancelOrderCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CancelOrderCommand for order: {Id}", request.Id);
            return false;
        }
    }
}
