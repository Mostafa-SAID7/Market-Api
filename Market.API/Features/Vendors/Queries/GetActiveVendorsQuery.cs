using MediatR;

namespace Market.API.Features.Vendors.Queries
{
    public class GetActiveVendorsQuery : IRequest<IEnumerable<VendorResponse>>
    {
    }

    public class GetActiveVendorsQueryHandler : IRequestHandler<GetActiveVendorsQuery, IEnumerable<VendorResponse>>
    {
        private readonly ILogger<GetActiveVendorsQueryHandler> _logger;

        public GetActiveVendorsQueryHandler(ILogger<GetActiveVendorsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<VendorResponse>> Handle(GetActiveVendorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetActiveVendorsQuery");
            return Enumerable.Empty<VendorResponse>();
        }
    }
}
