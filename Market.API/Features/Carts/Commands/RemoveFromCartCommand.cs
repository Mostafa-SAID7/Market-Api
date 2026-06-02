using MediatR;

namespace Market.API.Features.Carts.Commands
{
    /// <summary>
    /// Remove from cart command
    /// </summary>
    public class RemoveFromCartCommand : IRequest<CartResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Remove from cart command handler
    /// </summary>
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, CartResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RemoveFromCartCommandHandler> _logger;

        public RemoveFromCartCommandHandler(IMediator mediator, ILogger<RemoveFromCartCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<CartResponse> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RemoveFromCartCommand for user: {UserId}, product: {ProductId}", 
                request.UserId, request.ProductId);

            var result = await _mediator.Send(
                new RemoveFromCartInternalCommand { UserId = request.UserId, ProductId = request.ProductId }, 
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for removing from cart
    /// </summary>
    internal class RemoveFromCartInternalCommand : IRequest<CartResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
    }
}
