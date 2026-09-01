using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Vendors.Commands
{
    public class RejectVendorCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Reason { get; set; }
    }

    public class RejectVendorCommandHandler : IRequestHandler<RejectVendorCommand, bool>
    {
        private readonly ILogger<RejectVendorCommandHandler> _logger;

        public RejectVendorCommandHandler(ILogger<RejectVendorCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(RejectVendorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RejectVendorCommand for vendor: {Id}", request.Id);
            return false;
        }
    }
}



