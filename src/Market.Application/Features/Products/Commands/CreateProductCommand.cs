using MediatR;
using Market.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Commands
{
    /// <summary>
    /// Create product command
    /// </summary>
    public class CreateProductCommand : IRequest<ProductResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? SKU { get; set; }
    }

    /// <summary>
    /// Create product command handler
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IMediator mediator, ILogger<CreateProductCommandHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateProductCommand for product: {ProductName}", request.Name);

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                VendorId = request.VendorId,
                CategoryId = request.CategoryId,
                Price = request.Price,
                DiscountPrice = request.DiscountPrice,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl,
                SKU = request.SKU
            };

            // Send query to validate then create
            var result = await _mediator.Send(new CreateProductInternalCommand { Product = product }, cancellationToken);
            return result;
        }
    }

    /// <summary>
    /// Internal command for creating product (handles actual creation)
    /// </summary>
    internal class CreateProductInternalCommand : IRequest<ProductResponse>
    {
        public Product Product { get; set; } = null!;
    }
}



