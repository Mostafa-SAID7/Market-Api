using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Products.Queries
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductByIdQuery for product: {ProductId}", request.Id);

            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
            if (product == null)
                return null;

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ImageUrl = product.ImageUrl,
                Quantity = product.Quantity,
                Sold = product.Sold,
                CategoryId = product.CategoryId,
                VendorId = product.VendorId,
                AverageRating = product.AverageRating,
                ReviewCount = product.ReviewCount
            };
        }
    }
}
