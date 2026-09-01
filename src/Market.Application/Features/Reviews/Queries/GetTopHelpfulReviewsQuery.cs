using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Reviews.Queries
{
    public class GetTopHelpfulReviewsQuery : IRequest<IEnumerable<ReviewResponse>>
    {
        public int ProductId { get; set; }
        public int Count { get; set; } = 10;
    }

    public class GetTopHelpfulReviewsQueryHandler : IRequestHandler<GetTopHelpfulReviewsQuery, IEnumerable<ReviewResponse>>
    {
        private readonly ILogger<GetTopHelpfulReviewsQueryHandler> _logger;

        public GetTopHelpfulReviewsQueryHandler(ILogger<GetTopHelpfulReviewsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> Handle(GetTopHelpfulReviewsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetTopHelpfulReviewsQuery for product: {ProductId}", request.ProductId);
            return Enumerable.Empty<ReviewResponse>();
        }
    }
}



