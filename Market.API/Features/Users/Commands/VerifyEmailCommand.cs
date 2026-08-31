using MediatR;

namespace Market.API.Features.Users.Commands
{
    public class VerifyEmailCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
    {
        private readonly ILogger<VerifyEmailCommandHandler> _logger;

        public VerifyEmailCommandHandler(ILogger<VerifyEmailCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling VerifyEmailCommand for user: {Id}", request.Id);
            return false;
        }
    }
}
