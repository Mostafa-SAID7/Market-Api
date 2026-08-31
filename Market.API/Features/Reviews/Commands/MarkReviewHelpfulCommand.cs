using MediatR;

namespace Market.API.Features.Reviews.Commands
{
    public class MarkReviewHelpfulCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class MarkReviewHelpfulCommandHandler : IRequestHandler<MarkReviewHelpfulCommand, bool>
    {
        private readonly ILogger<MarkReviewHelpfulCommandHandler> _logger;

        public MarkReviewHelpfulCommandHandler(ILogger<MarkReviewHelpfulCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(MarkReviewHelpfulCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling MarkReviewHelpfulCommand for review: {Id}", request.Id);
            return false;
        }
    }
}
