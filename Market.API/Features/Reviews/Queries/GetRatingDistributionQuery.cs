using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetRatingDistributionQuery : IRequest<Dictionary<int, int>>
    {
        public int ProductId { get; set; }
    }

    public class GetRatingDistributionQueryHandler : IRequestHandler<GetRatingDistributionQuery, Dictionary<int, int>>
    {
        private readonly ILogger<GetRatingDistributionQueryHandler> _logger;

        public GetRatingDistributionQueryHandler(ILogger<GetRatingDistributionQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<Dictionary<int, int>> Handle(GetRatingDistributionQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetRatingDistributionQuery for product: {ProductId}", request.ProductId);
            return new Dictionary<int, int>();
        }
    }
}
