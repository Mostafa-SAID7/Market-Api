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

    /// <summary>
    /// Internal add to cart command handler
    /// </summary>
    internal class AddToCartInternalCommandHandler : IRequestHandler<AddToCartInternalCommand, CartResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddToCartInternalCommandHandler> _logger;

        public AddToCartInternalCommandHandler(IUnitOfWork unitOfWork, ILogger<AddToCartInternalCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CartResponse> Handle(AddToCartInternalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling AddToCartInternalCommand for user: {UserId}, product: {ProductId}, vendor: {VendorId}",
                request.UserId, request.Item.ProductId, request.Item.VendorId);

            // Get or create cart for user
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId)
                      ?? new Domain.Entities.Cart { UserId = request.UserId };

            // Use domain method to add item - this will handle the ProductId uniqueness logic.
            cart.AddItem(request.Item);

            if (cart.Id == 0)
            {
                await _unitOfWork.Carts.CreateAsync(cart, cancellationToken);
            }
            else
            {
                await _unitOfWork.Carts.UpdateAsync(cart, cancellationToken);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return MapToResponse(cart);
        }

        private CartResponse MapToResponse(Domain.Entities.Cart cart)
        {
            return new CartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    SubTotal = i.SubTotal,
                    VendorId = i.VendorId,
                    ImageUrl = i.ImageUrl
                }).ToList(),
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };
        }
    }
}

