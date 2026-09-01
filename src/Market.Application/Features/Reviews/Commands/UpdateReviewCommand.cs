using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Reviews.Commands
{
    /// <summary>
    /// Update review command
    /// </summary>
    public class UpdateReviewCommand : IRequest<ReviewResponse>
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
    }

    /// <summary>
    /// Update review command handler
    /// </summary>
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateReviewCommandHandler> _logger;

        public UpdateReviewCommandHandler(IMediator mediator, ILogger<UpdateReviewCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ReviewResponse> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateReviewCommand for review: {ReviewId}", request.Id);

            var result = await _mediator.Send(
                new UpdateReviewInternalCommand
                {
                    Id = request.Id,
                    RatingValue = request.RatingValue,
                    Title = request.Title,
                    Comment = request.Comment,
                    ImageUrls = request.ImageUrls
                },
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for updating review
    /// </summary>
    internal class UpdateReviewInternalCommand : IRequest<ReviewResponse>
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
    }
}



