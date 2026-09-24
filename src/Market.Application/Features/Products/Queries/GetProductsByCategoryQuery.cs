using MediatR;
using Market.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries
{
    /// <summary>
    /// Get products by category query
    /// </summary>
    public class GetProductsByCategoryQuery : IRequest<List<ProductResponse>>
    {
        public int CategoryId { get; set; }
    }

    /// <summary>
    /// Get products by category query handler
    /// </summary>
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, List<ProductResponse>>
    {
        private readonly IProductReadService _productReadService;
        private readonly ILogger<GetProductsByCategoryQueryHandler> _logger;

        public GetProductsByCategoryQueryHandler(IProductReadService productReadService, ILogger<GetProductsByCategoryQueryHandler> logger)
        {
            _productReadService = productReadService;
            _logger = logger;
        }

        public async Task<List<ProductResponse>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductsByCategoryQuery for category: {CategoryId}", request.CategoryId);

            var products = await _productReadService.GetProductsByCategoryAsync(request.CategoryId, cancellationToken);
            return products.ToList();
        }
    }
}



