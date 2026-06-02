using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Carts.Queries
{
    /// <summary>
    /// Get cart by user id query
    /// </summary>
    public class GetCartByUserIdQuery : IRequest<CartResponse?>
    {
        public string UserId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Get cart by user id query handler
    /// </summary>
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, CartResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetCartByUserIdQueryHandler> _logger;

        public GetCartByUserIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCartByUserIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CartResponse?> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCartByUserIdQuery for user: {UserId}", request.UserId);

            var carts = await _unitOfWork.Carts.GetAllAsync();
            var cart = carts.FirstOrDefault(c => c.UserId == request.UserId);
            
            if (cart == null)
                return null;

            return new CartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    VendorId = i.VendorId,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.ImageUrl,
                    SubTotal = i.SubTotal
                }).ToList(),
                TotalPrice = cart.SubTotal,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };
        }
    }
}
