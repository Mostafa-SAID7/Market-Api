using MediatR;
using Market.API.Models.Entities;

namespace Market.API.Features.Reviews.Commands
{
    /// <summary>
    /// Create review command
    /// </summary>
    public class CreateReviewCommand : IRequest<ReviewResponse>
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        public int RatingValue { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
    }

    /// <summary>
    /// Create review command handler
    /// </summary>
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(IMediator mediator, ILogger<CreateReviewCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ReviewResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateReviewCommand for product: {ProductId}", request.ProductId);

            var review = new Review
            {
                ProductId = request.ProductId,
                VendorId = request.VendorId,
                CustomerId = request.CustomerId,
                RatingValue = request.RatingValue,
                Title = request.Title,
                Comment = request.Comment
            };

            // Add images as ReviewImage entities
            foreach (var imageUrl in request.ImageUrls)
            {
                review.Images.Add(new ReviewImage { ImageUrl = imageUrl });
            }

            var result = await _mediator.Send(new CreateReviewInternalCommand { Review = review }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for creating review
    /// </summary>
    internal class CreateReviewInternalCommand : IRequest<ReviewResponse>
    {
        public Review Review { get; set; } = null!;
    }
}
