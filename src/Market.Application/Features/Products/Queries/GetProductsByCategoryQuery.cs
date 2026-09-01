using MediatR;
using Market.Domain.Repositories;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetProductsByCategoryQueryHandler> _logger;

        public GetProductsByCategoryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductsByCategoryQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ProductResponse>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductsByCategoryQuery for category: {CategoryId}", request.CategoryId);

            var products = await _unitOfWork.Products.GetAllAsync();
            return products
                .Where(p => p.CategoryId == request.CategoryId)
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
                    CategoryId = p.CategoryId,
                    VendorId = p.VendorId,
                    AverageRating = p.AverageRating,
                    ReviewCount = p.ReviewCount
                })
                .ToList();
        }
    }
}



