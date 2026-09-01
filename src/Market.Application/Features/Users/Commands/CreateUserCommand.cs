using MediatR;
using Market.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Users.Commands
{
    /// <summary>
    /// Create user command
    /// </summary>
    public class CreateUserCommand : IRequest<UserResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// Create user command handler
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(IMediator mediator, ILogger<CreateUserCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateUserCommand for user: {Email}", request.Email);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            // Send internal command to create
            var result = await _mediator.Send(new CreateUserInternalCommand { User = user }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for creating user (handles actual creation)
    /// </summary>
    internal class CreateUserInternalCommand : IRequest<UserResponse>
    {
        public User User { get; set; } = null!;
    }
}



