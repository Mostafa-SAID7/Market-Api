using MediatR;
using Market.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Users.Queries
{
    public class GetUsersByRoleQuery : IRequest<IEnumerable<UserResponse>>
    {
        public UserRole Role { get; set; }
    }

    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserResponse>>
    {
        private readonly ILogger<GetUsersByRoleQueryHandler> _logger;

        public GetUsersByRoleQueryHandler(ILogger<GetUsersByRoleQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUsersByRoleQuery for role: {Role}", request.Role);
            return Enumerable.Empty<UserResponse>();
        }
    }
}



