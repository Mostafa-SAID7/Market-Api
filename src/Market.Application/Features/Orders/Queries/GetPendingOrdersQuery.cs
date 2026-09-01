using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Orders.Queries
{
    public class GetPendingOrdersQuery : IRequest<IEnumerable<OrderResponse>>
    {
    }

    public class GetPendingOrdersQueryHandler : IRequestHandler<GetPendingOrdersQuery, IEnumerable<OrderResponse>>
    {
        private readonly ILogger<GetPendingOrdersQueryHandler> _logger;

        public GetPendingOrdersQueryHandler(ILogger<GetPendingOrdersQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetPendingOrdersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetPendingOrdersQuery");
            return Enumerable.Empty<OrderResponse>();
        }
    }
}



