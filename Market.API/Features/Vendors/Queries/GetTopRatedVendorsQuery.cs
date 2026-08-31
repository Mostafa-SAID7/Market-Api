using MediatR;

namespace Market.API.Features.Vendors.Queries
{
    public class GetTopRatedVendorsQuery : IRequest<IEnumerable<VendorResponse>>
    {
        public int Count { get; set; } = 10;
    }

    public class GetTopRatedVendorsQueryHandler : IRequestHandler<GetTopRatedVendorsQuery, IEnumerable<VendorResponse>>
    {
        private readonly ILogger<GetTopRatedVendorsQueryHandler> _logger;

        public GetTopRatedVendorsQueryHandler(ILogger<GetTopRatedVendorsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<VendorResponse>> Handle(GetTopRatedVendorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetTopRatedVendorsQuery");
            return Enumerable.Empty<VendorResponse>();
        }
    }
}
