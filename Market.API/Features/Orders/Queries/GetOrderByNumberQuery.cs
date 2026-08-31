using MediatR;

namespace Market.API.Features.Orders.Queries
{
    public class GetOrderByNumberQuery : IRequest<OrderResponse?>
    {
        public string OrderNumber { get; set; } = string.Empty;
    }

    public class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQuery, OrderResponse?>
    {
        private readonly ILogger<GetOrderByNumberQueryHandler> _logger;

        public GetOrderByNumberQueryHandler(ILogger<GetOrderByNumberQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<OrderResponse?> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetOrderByNumberQuery for order: {OrderNumber}", request.OrderNumber);
            return null;
        }
    }
}
