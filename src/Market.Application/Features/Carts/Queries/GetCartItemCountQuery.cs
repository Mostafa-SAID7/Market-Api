using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Queries
{
    public class GetCartItemCountQuery : IRequest<int>
    {
        public int UserId { get; set; }
    }

    public class GetCartItemCountQueryHandler : IRequestHandler<GetCartItemCountQuery, int>
    {
        private readonly ILogger<GetCartItemCountQueryHandler> _logger;

        public GetCartItemCountQueryHandler(ILogger<GetCartItemCountQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<int> Handle(GetCartItemCountQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCartItemCountQuery for user: {UserId}", request.UserId);
            return 0;
        }
    }
}



