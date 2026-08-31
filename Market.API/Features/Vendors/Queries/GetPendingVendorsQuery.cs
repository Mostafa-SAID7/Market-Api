using MediatR;

namespace Market.API.Features.Vendors.Queries
{
    public class GetPendingVendorsQuery : IRequest<IEnumerable<VendorResponse>>
    {
    }

    public class GetPendingVendorsQueryHandler : IRequestHandler<GetPendingVendorsQuery, IEnumerable<VendorResponse>>
    {
        private readonly ILogger<GetPendingVendorsQueryHandler> _logger;

        public GetPendingVendorsQueryHandler(ILogger<GetPendingVendorsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<VendorResponse>> Handle(GetPendingVendorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetPendingVendorsQuery");
            return Enumerable.Empty<VendorResponse>();
        }
    }
}
