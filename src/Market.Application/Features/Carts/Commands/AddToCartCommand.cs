using MediatR;
using Market.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Commands
{
    /// <summary>
    /// Add to cart command
    /// </summary>
    public class AddToCartCommand : IRequest<CartResponse>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int VendorId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
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
                VendorId = request.VendorId,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                Price = request.Price,
                ImageUrl = request.ImageUrl
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
        public int UserId { get; set; }
        public CartItem Item { get; set; } = null!;
    }
}


