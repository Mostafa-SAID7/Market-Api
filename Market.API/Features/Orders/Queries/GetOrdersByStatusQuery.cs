using MediatR;
using Market.API.Models.Enums;

namespace Market.API.Features.Orders.Queries
{
    public class GetOrdersByStatusQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public OrderStatus Status { get; set; }
    }

    public class GetOrdersByStatusQueryHandler : IRequestHandler<GetOrdersByStatusQuery, IEnumerable<OrderResponse>>
    {
        private readonly ILogger<GetOrdersByStatusQueryHandler> _logger;

        public GetOrdersByStatusQueryHandler(ILogger<GetOrdersByStatusQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetOrdersByStatusQuery for status: {Status}", request.Status);
            return Enumerable.Empty<OrderResponse>();
        }
    }
}
