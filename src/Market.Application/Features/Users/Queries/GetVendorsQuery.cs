using MediatR;
using Market.Application.Features.Vendors;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Users.Queries
{
    public class GetVendorsQuery : IRequest<IEnumerable<VendorResponse>>
    {
    }

    public class GetVendorsQueryHandler : IRequestHandler<GetVendorsQuery, IEnumerable<VendorResponse>>
    {
        private readonly ILogger<GetVendorsQueryHandler> _logger;

        public GetVendorsQueryHandler(ILogger<GetVendorsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<VendorResponse>> Handle(GetVendorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetVendorsQuery");
            return Enumerable.Empty<VendorResponse>();
        }
    }
}



