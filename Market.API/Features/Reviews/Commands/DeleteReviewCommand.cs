using MediatR;

namespace Market.API.Features.Reviews.Commands
{
    /// <summary>
    /// Delete review command
    /// </summary>
    public class DeleteReviewCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Delete review command handler
    /// </summary>
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;

        public DeleteReviewCommandHandler(IMediator mediator, ILogger<DeleteReviewCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteReviewCommand for review: {ReviewId}", request.Id);

            var result = await _mediator.Send(new DeleteReviewInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for deleting review
    /// </summary>
    internal class DeleteReviewInternalCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
