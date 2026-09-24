using Market.Domain.Entities;
using Market.Domain.Repositories;
using MediatR;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.Users.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                IsEmailVerified = user.IsEmailVerified,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}



