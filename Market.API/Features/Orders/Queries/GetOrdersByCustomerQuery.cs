using MediatR;

namespace Market.API.Features.Orders.Queries
{
    public class GetOrdersByCustomerQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public int CustomerId { get; set; }
    }

    public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderResponse>>
    {
        private readonly ILogger<GetOrdersByCustomerQueryHandler> _logger;

        public GetOrdersByCustomerQueryHandler(ILogger<GetOrdersByCustomerQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetOrdersByCustomerQuery for customer: {CustomerId}", request.CustomerId);
            return Enumerable.Empty<OrderResponse>();
        }
    }
}
