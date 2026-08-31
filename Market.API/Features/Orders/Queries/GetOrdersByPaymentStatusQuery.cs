using MediatR;
using Market.API.Models.Enums;

namespace Market.API.Features.Orders.Queries
{
    public class GetOrdersByPaymentStatusQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public PaymentStatus PaymentStatus { get; set; }
    }

    public class GetOrdersByPaymentStatusQueryHandler : IRequestHandler<GetOrdersByPaymentStatusQuery, IEnumerable<OrderResponse>>
    {
        private readonly ILogger<GetOrdersByPaymentStatusQueryHandler> _logger;

        public GetOrdersByPaymentStatusQueryHandler(ILogger<GetOrdersByPaymentStatusQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersByPaymentStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetOrdersByPaymentStatusQuery for payment status: {PaymentStatus}", request.PaymentStatus);
            return Enumerable.Empty<OrderResponse>();
        }
    }
}
