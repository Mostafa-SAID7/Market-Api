using MediatR;
using Market.API.Models.Entities;

namespace Market.API.Features.Carts.Commands
{
    /// <summary>
    /// Add to cart command
    /// </summary>
    public class AddToCartCommand : IRequest<CartResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Add to cart command handler
    /// </summary>
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddToCartCommandHandler> _logger;

        public AddToCartCommandHandler(IMediator mediator, ILogger<AddToCartCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<CartResponse> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling AddToCartCommand for user: {UserId}, product: {ProductId}", 
                request.UserId, request.ProductId);

            var cartItem = new CartItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = request.Price
            };

            var result = await _mediator.Send(
                new AddToCartInternalCommand { UserId = request.UserId, Item = cartItem }, 
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for adding to cart
    /// </summary>
    internal class AddToCartInternalCommand : IRequest<CartResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public CartItem Item { get; set; } = null!;
    }
}
