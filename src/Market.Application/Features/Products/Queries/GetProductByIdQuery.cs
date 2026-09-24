using MediatR;
using Market.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries
{
    /// <summary>
    /// Get product by id query
    /// </summary>
    public class GetProductByIdQuery : IRequest<ProductResponse?>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Get product by id query handler
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
    {
        private readonly IProductReadService _productReadService;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;

        public GetProductByIdQueryHandler(IProductReadService productReadService, ILogger<GetProductByIdQueryHandler> logger)
        {
            _productReadService = productReadService;
            _logger = logger;
        }

        public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductByIdQuery for product: {ProductId}", request.Id);

            return await _productReadService.GetProductByIdAsync(request.Id, cancellationToken);
        }
    }
}



