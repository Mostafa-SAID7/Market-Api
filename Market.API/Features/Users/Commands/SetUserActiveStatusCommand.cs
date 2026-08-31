using MediatR;

namespace Market.API.Features.Users.Commands
{
    /// <summary>
    /// Set user active status command
    /// </summary>
    public class SetUserActiveStatusCommand : IRequest<UserResponse>
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Set user active status command handler
    /// </summary>
    public class SetUserActiveStatusCommandHandler : IRequestHandler<SetUserActiveStatusCommand, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetUserActiveStatusCommandHandler> _logger;

        public SetUserActiveStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<SetUserActiveStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserResponse> Handle(SetUserActiveStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting user {UserId} active status to {IsActive}", request.Id, request.IsActive);

            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            user.IsActive = request.IsActive;
            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return MapToResponse(user);
        }

        private UserResponse MapToResponse(Market.API.Models.Entities.User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
