using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Users.Commands
{
    /// <summary>
    /// Update user command
    /// </summary>
    public class UpdateUserCommand : IRequest<UserResponse>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// Update user command handler
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IMediator mediator, ILogger<UpdateUserCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateUserCommand for user: {UserId}", request.Id);

            var result = await _mediator.Send(
                new UpdateUserInternalCommand
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                },
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for updating user
    /// </summary>
    internal class UpdateUserInternalCommand : IRequest<UserResponse>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}



