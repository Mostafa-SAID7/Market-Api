using MediatR;
using Market.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries
{
    /// <summary>
    /// Get all products query
    /// </summary>
    public class GetAllProductsQuery : IRequest<List<ProductResponse>>
    {
    }

    /// <summary>
    /// Get all products query handler
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
    {
        private readonly IProductReadService _productReadService;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IProductReadService productReadService, ILogger<GetAllProductsQueryHandler> logger)
        {
            _productReadService = productReadService;
            _logger = logger;
        }

        public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllProductsQuery");

            var products = await _productReadService.GetAllProductsAsync(cancellationToken);
            return products.ToList();
        }
    }
}



