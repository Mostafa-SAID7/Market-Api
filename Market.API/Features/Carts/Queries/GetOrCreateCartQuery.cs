using MediatR;

namespace Market.API.Features.Carts.Queries
{
    public class GetOrCreateCartQuery : IRequest<CartResponse?>
    {
        public int UserId { get; set; }
    }

    public class GetOrCreateCartQueryHandler : IRequestHandler<GetOrCreateCartQuery, CartResponse?>
    {
        private readonly ILogger<GetOrCreateCartQueryHandler> _logger;

        public GetOrCreateCartQueryHandler(ILogger<GetOrCreateCartQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<CartResponse?> Handle(GetOrCreateCartQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetOrCreateCartQuery for user: {UserId}", request.UserId);
            return null;
        }
    }
}
