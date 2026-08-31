using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetReviewsByCustomerQuery : IRequest<IEnumerable<ReviewResponse>>
    {
        public int CustomerId { get; set; }
    }

    public class GetReviewsByCustomerQueryHandler : IRequestHandler<GetReviewsByCustomerQuery, IEnumerable<ReviewResponse>>
    {
        private readonly ILogger<GetReviewsByCustomerQueryHandler> _logger;

        public GetReviewsByCustomerQueryHandler(ILogger<GetReviewsByCustomerQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> Handle(GetReviewsByCustomerQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetReviewsByCustomerQuery for customer: {CustomerId}", request.CustomerId);
            return Enumerable.Empty<ReviewResponse>();
        }
    }
}
