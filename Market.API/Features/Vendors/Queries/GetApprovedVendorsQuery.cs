using MediatR;

namespace Market.API.Features.Vendors.Queries
{
    public class GetApprovedVendorsQuery : IRequest<IEnumerable<VendorResponse>>
    {
    }

    public class GetApprovedVendorsQueryHandler : IRequestHandler<GetApprovedVendorsQuery, IEnumerable<VendorResponse>>
    {
        private readonly ILogger<GetApprovedVendorsQueryHandler> _logger;

        public GetApprovedVendorsQueryHandler(ILogger<GetApprovedVendorsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<VendorResponse>> Handle(GetApprovedVendorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetApprovedVendorsQuery");
            return Enumerable.Empty<VendorResponse>();
        }
    }
}
