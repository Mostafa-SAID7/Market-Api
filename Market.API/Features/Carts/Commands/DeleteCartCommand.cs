using MediatR;

namespace Market.API.Features.Carts.Commands
{
    public class DeleteCartCommand : IRequest<bool>
    {
        public int UserId { get; set; }
    }

    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, bool>
    {
        private readonly ILogger<DeleteCartCommandHandler> _logger;

        public DeleteCartCommandHandler(ILogger<DeleteCartCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCartCommand for user: {UserId}", request.UserId);
            return false;
        }
    }
}
