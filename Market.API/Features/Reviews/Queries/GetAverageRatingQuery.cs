using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetAverageRatingQuery : IRequest<double>
    {
        public int ProductId { get; set; }
    }

    public class GetAverageRatingQueryHandler : IRequestHandler<GetAverageRatingQuery, double>
    {
        private readonly ILogger<GetAverageRatingQueryHandler> _logger;

        public GetAverageRatingQueryHandler(ILogger<GetAverageRatingQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<double> Handle(GetAverageRatingQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAverageRatingQuery for product: {ProductId}", request.ProductId);
            return 0;
        }
    }
}
