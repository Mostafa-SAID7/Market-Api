using MediatR;

namespace Market.API.Features.Users.Queries
{
    public class GetUserByEmailQuery : IRequest<UserResponse?>
    {
        public string Email { get; set; } = string.Empty;
    }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserResponse?>
    {
        private readonly ILogger<GetUserByEmailQueryHandler> _logger;

        public GetUserByEmailQueryHandler(ILogger<GetUserByEmailQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<UserResponse?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetUserByEmailQuery for email: {Email}", request.Email);
            return null;
        }
    }
}
