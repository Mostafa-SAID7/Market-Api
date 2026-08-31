using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetReviewsByRatingQuery : IRequest<IEnumerable<ReviewResponse>>
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
    }

    public class GetReviewsByRatingQueryHandler : IRequestHandler<GetReviewsByRatingQuery, IEnumerable<ReviewResponse>>
    {
        private readonly ILogger<GetReviewsByRatingQueryHandler> _logger;

        public GetReviewsByRatingQueryHandler(ILogger<GetReviewsByRatingQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> Handle(GetReviewsByRatingQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetReviewsByRatingQuery for product: {ProductId}, rating: {Rating}", request.ProductId, request.Rating);
            return Enumerable.Empty<ReviewResponse>();
        }
    }
}
