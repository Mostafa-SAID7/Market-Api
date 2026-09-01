using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Reviews.Queries
{
    public class GetReviewsByProductQuery : IRequest<IEnumerable<ReviewResponse>>
    {
        public int ProductId { get; set; }
    }

    public class GetReviewsByProductQueryHandler : IRequestHandler<GetReviewsByProductQuery, IEnumerable<ReviewResponse>>
    {
        private readonly ILogger<GetReviewsByProductQueryHandler> _logger;

        public GetReviewsByProductQueryHandler(ILogger<GetReviewsByProductQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> Handle(GetReviewsByProductQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetReviewsByProductQuery for product: {ProductId}", request.ProductId);
            return Enumerable.Empty<ReviewResponse>();
        }
    }
}



