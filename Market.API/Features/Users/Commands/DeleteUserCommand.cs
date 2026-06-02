using MediatR;

namespace Market.API.Features.Users.Commands
{
    /// <summary>
    /// Delete user command
    /// </summary>
    public class DeleteUserCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Delete user command handler
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(IMediator mediator, ILogger<DeleteUserCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteUserCommand for user: {UserId}", request.Id);

            var result = await _mediator.Send(new DeleteUserInternalCommand { Id = request.Id }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for deleting user
    /// </summary>
    internal class DeleteUserInternalCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
