using MediatR;

namespace Market.API.Features.Carts.Queries
{
    public class GetCartItemsQuery : IRequest<IEnumerable<CartItemResponse>>
    {
        public int UserId { get; set; }
    }

    public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, IEnumerable<CartItemResponse>>
    {
        private readonly ILogger<GetCartItemsQueryHandler> _logger;

        public GetCartItemsQueryHandler(ILogger<GetCartItemsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<CartItemResponse>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCartItemsQuery for user: {UserId}", request.UserId);
            return Enumerable.Empty<CartItemResponse>();
        }
    }
}
