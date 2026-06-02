using MediatR;

namespace Market.API.Features.Products.Commands
{
    /// <summary>
    /// Update product command
    /// </summary>
    public class UpdateProductCommand : IRequest<ProductResponse>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Update product command handler
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IMediator mediator, ILogger<UpdateProductCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateProductCommand for product: {ProductId}", request.Id);

            var result = await _mediator.Send(
                new UpdateProductInternalCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    DiscountPrice = request.DiscountPrice,
                    Quantity = request.Quantity,
                    ImageUrl = request.ImageUrl
                },
                cancellationToken);

            return result;
        }
    }

    /// <summary>
    /// Internal command for updating product
    /// </summary>
    internal class UpdateProductInternalCommand : IRequest<ProductResponse>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
