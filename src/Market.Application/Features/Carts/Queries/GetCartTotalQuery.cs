using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Queries
{
    public class GetCartTotalQuery : IRequest<decimal>
    {
        public int UserId { get; set; }
    }

    public class GetCartTotalQueryHandler : IRequestHandler<GetCartTotalQuery, decimal>
    {
        private readonly ILogger<GetCartTotalQueryHandler> _logger;

        public GetCartTotalQueryHandler(ILogger<GetCartTotalQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<decimal> Handle(GetCartTotalQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCartTotalQuery for user: {UserId}", request.UserId);
            return 0M;
        }
    }
}



