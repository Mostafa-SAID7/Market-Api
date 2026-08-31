using MediatR;

namespace Market.API.Features.Reviews.Queries
{
    public class GetReviewsByVendorQuery : IRequest<IEnumerable<ReviewResponse>>
    {
        public int VendorId { get; set; }
    }

    public class GetReviewsByVendorQueryHandler : IRequestHandler<GetReviewsByVendorQuery, IEnumerable<ReviewResponse>>
    {
        private readonly ILogger<GetReviewsByVendorQueryHandler> _logger;

        public GetReviewsByVendorQueryHandler(ILogger<GetReviewsByVendorQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> Handle(GetReviewsByVendorQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetReviewsByVendorQuery for vendor: {VendorId}", request.VendorId);
            return Enumerable.Empty<ReviewResponse>();
        }
    }
}
