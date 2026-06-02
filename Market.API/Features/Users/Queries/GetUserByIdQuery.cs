using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Users.Queries
{
    /// <summary>
    /// Get user by id query
    /// </summary>
    public class GetUserByIdQuery : IRequest<UserResponse?>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Get user by id query handler
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserByIdQuery for user: {UserId}", request.Id);

            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
                return null;

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                VendorId = user.VendorId,
                IsActive = user.IsActive,
                IsEmailVerified = user.IsEmailVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
