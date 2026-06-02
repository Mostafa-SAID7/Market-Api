using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Users.Queries
{
    /// <summary>
    /// Get all users query
    /// </summary>
    public class GetAllUsersQuery : IRequest<List<UserResponse>>
    {
    }

    /// <summary>
    /// Get all users query handler
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUsersQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllUsersQuery");

            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                VendorId = u.VendorId,
                IsActive = u.IsActive,
                IsEmailVerified = u.IsEmailVerified,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }
    }
}
