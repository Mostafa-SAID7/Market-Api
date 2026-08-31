using MediatR;

namespace Market.API.Features.Vendors.Queries
{
    public class GetVendorByUserIdQuery : IRequest<VendorResponse?>
    {
        public int UserId { get; set; }
    }

    public class GetVendorByUserIdQueryHandler : IRequestHandler<GetVendorByUserIdQuery, VendorResponse?>
    {
        private readonly ILogger<GetVendorByUserIdQueryHandler> _logger;

        public GetVendorByUserIdQueryHandler(ILogger<GetVendorByUserIdQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<VendorResponse?> Handle(GetVendorByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetVendorByUserIdQuery for user: {UserId}", request.UserId);
            return null;
        }
    }
}
