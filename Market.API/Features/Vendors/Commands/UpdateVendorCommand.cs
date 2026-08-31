using MediatR;

namespace Market.API.Features.Vendors.Commands
{
    /// <summary>
    /// Update vendor command
    /// </summary>
    public class UpdateVendorCommand : IRequest<VendorResponse>
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string StoreDescription { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Banner { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
    }

    /// <summary>
    /// Update vendor command handler
    /// </summary>
    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, VendorResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateVendorCommandHandler> _logger;

        public UpdateVendorCommandHandler(IMediator mediator, ILogger<UpdateVendorCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<VendorResponse> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateVendorCommand for vendor: {VendorId}", request.Id);

            var result = await _mediator.Send(
                new UpdateVendorInternalCommand
                {
                    Id = request.Id,
                    StoreName = request.StoreName,
                    StoreDescription = request.StoreDescription,
                    Logo = request.Logo,
                    Banner = request.Banner,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    City = request.City,
                    Country = request.Country,
                    ZipCode = request.ZipCode
                },
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for updating vendor
    /// </summary>
    internal class UpdateVendorInternalCommand : IRequest<VendorResponse>
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string StoreDescription { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Banner { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
    }
}
