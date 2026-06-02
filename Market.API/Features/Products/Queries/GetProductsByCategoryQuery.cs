using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Products.Queries
{
    /// <summary>
    /// Get products by category query
    /// </summary>
    public class GetProductsByCategoryQuery : IRequest<List<ProductResponse>>
    {
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// Get products by category query handler
    /// </summary>
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, List<ProductResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetProductsByCategoryQueryHandler> _logger;

        public GetProductsByCategoryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductsByCategoryQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ProductResponse>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductsByCategoryQuery for category: {Category}", request.Category);

            var products = await _unitOfWork.Products.GetAllAsync();
            return products
                .Where(p => p.Category == request.Category)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    ImageUrl = p.ImageUrl,
                    Quantity = p.Quantity,
                    Sold = p.Sold,
                    Category = p.Category,
                    VendorId = p.VendorId,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                })
                .ToList();
        }
    }
}
