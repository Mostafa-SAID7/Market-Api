using MediatR;
using Market.Domain.Repositories;
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

            var p = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
            if (p == null) return null;

            return new ProductResponse
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
            };
        }
    }
}



