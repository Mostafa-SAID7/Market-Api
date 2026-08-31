using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetVerifiedReviewsQuery : IRequest<IEnumerable<ReviewResponse>>
    {
        public int ProductId { get; set; }
    }

    public class GetVerifiedReviewsQueryHandler : IRequestHandler<GetVerifiedReviewsQuery, IEnumerable<ReviewResponse>>
    {
        private readonly ILogger<GetVerifiedReviewsQueryHandler> _logger;

        public GetVerifiedReviewsQueryHandler(ILogger<GetVerifiedReviewsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> Handle(GetVerifiedReviewsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetVerifiedReviewsQuery for product: {ProductId}", request.ProductId);
            return Enumerable.Empty<ReviewResponse>();
        }
    }
}
