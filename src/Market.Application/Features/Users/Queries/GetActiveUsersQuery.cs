using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Users.Queries
{
    public class GetActiveUsersQuery : IRequest<IEnumerable<UserResponse>>
    {
    }

    public class GetActiveUsersQueryHandler : IRequestHandler<GetActiveUsersQuery, IEnumerable<UserResponse>>
    {
        private readonly ILogger<GetActiveUsersQueryHandler> _logger;

        public GetActiveUsersQueryHandler(ILogger<GetActiveUsersQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetActiveUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetActiveUsersQuery");
            return Enumerable.Empty<UserResponse>();
        }
    }
}



