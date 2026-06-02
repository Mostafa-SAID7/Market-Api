using MediatR;

namespace Market.API.Features.Carts.Commands
{
    /// <summary>
    /// Clear cart command
    /// </summary>
    public class ClearCartCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Clear cart command handler
    /// </summary>
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClearCartCommandHandler> _logger;

        public ClearCartCommandHandler(IMediator mediator, ILogger<ClearCartCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling ClearCartCommand for user: {UserId}", request.UserId);

            var result = await _mediator.Send(new ClearCartInternalCommand { UserId = request.UserId }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for clearing cart
    /// </summary>
    internal class ClearCartInternalCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
