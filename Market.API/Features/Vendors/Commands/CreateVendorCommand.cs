using MediatR;
using Market.API.Models.Entities;

namespace Market.API.Features.Vendors.Commands
{
    /// <summary>
    /// Create vendor command
    /// </summary>
    public class CreateVendorCommand : IRequest<VendorResponse>
    {
        public string UserId { get; set; } = string.Empty;
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
    /// Create vendor command handler
    /// </summary>
    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, VendorResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateVendorCommandHandler> _logger;

        public CreateVendorCommandHandler(IMediator mediator, ILogger<CreateVendorCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<VendorResponse> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateVendorCommand for vendor: {StoreName}", request.StoreName);

            var vendor = new Vendor
            {
                UserId = request.UserId,
                StoreName = request.StoreName,
                StoreDescription = request.StoreDescription,
                Logo = request.Logo ?? string.Empty,
                Banner = request.Banner ?? string.Empty,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                ZipCode = request.ZipCode,
                IsApproved = false
            };

            var result = await _mediator.Send(new CreateVendorInternalCommand { Vendor = vendor }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for creating vendor
    /// </summary>
    internal class CreateVendorInternalCommand : IRequest<VendorResponse>
    {
        public Vendor Vendor { get; set; } = null!;
    }
}
