using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Orders.Queries
{
    public class GetRecentOrdersQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public int Count { get; set; } = 50;
    }

    public class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery, IEnumerable<OrderResponse>>
    {
        private readonly ILogger<GetRecentOrdersQueryHandler> _logger;

        public GetRecentOrdersQueryHandler(ILogger<GetRecentOrdersQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetRecentOrdersQuery for last {Count} orders", request.Count);
            return Enumerable.Empty<OrderResponse>();
        }
    }
}



