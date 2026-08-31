using MediatR;

namespace Market.API.Features.Products.Commands
{
    /// <summary>
    /// Delete product command
    /// </summary>
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Delete product command handler
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(IMediator mediator, ILogger<DeleteProductCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteProductCommand for product: {ProductId}", request.Id);

            var result = await _mediator.Send(new DeleteProductInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for deleting product
    /// </summary>
    internal class DeleteProductInternalCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
