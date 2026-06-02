using MediatR;

namespace Market.API.Features.Categories.Commands
{
    /// <summary>
    /// Delete category command
    /// </summary>
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Delete category command handler
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(IMediator mediator, ILogger<DeleteCategoryCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCategoryCommand for category: {CategoryId}", request.Id);

            var result = await _mediator.Send(new DeleteCategoryInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for deleting category
    /// </summary>
    internal class DeleteCategoryInternalCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
