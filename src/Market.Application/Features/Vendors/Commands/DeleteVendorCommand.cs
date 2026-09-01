using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Vendors.Commands
{
    /// <summary>
    /// Delete vendor command
    /// </summary>
    public class DeleteVendorCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Delete vendor command handler
    /// </summary>
    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteVendorCommandHandler> _logger;

        public DeleteVendorCommandHandler(IMediator mediator, ILogger<DeleteVendorCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteVendorCommand for vendor: {VendorId}", request.Id);

            var result = await _mediator.Send(new DeleteVendorInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for deleting vendor
    /// </summary>
    internal class DeleteVendorInternalCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}



