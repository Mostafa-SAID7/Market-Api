using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Vendors.Commands
{
    /// <summary>
    /// Approve vendor command
    /// </summary>
    public class ApproveVendorCommand : IRequest<VendorResponse>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Approve vendor command handler
    /// </summary>
    public class ApproveVendorCommandHandler : IRequestHandler<ApproveVendorCommand, VendorResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApproveVendorCommandHandler> _logger;

        public ApproveVendorCommandHandler(IMediator mediator, ILogger<ApproveVendorCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<VendorResponse> Handle(ApproveVendorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling ApproveVendorCommand for vendor: {VendorId}", request.Id);

            var result = await _mediator.Send(new ApproveVendorInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for approving vendor
    /// </summary>
    internal class ApproveVendorInternalCommand : IRequest<VendorResponse>
    {
        public int Id { get; set; }
    }
}



