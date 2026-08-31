using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Products.Queries
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllProductsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllProductsQuery");

            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                VendorId = p.VendorId,
                Quantity = p.Quantity,
                Sold = p.Sold,
                AverageRating = p.AverageRating,
                ReviewCount = p.ReviewCount
            }).ToList();
        }
    }
}
