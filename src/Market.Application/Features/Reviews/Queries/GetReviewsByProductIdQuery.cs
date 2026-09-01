using MediatR;
using Market.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Reviews.Queries
{
    /// <summary>
    /// Get reviews by product id query
    /// </summary>
    public class GetReviewsByProductIdQuery : IRequest<List<ReviewResponse>>
    {
        public int ProductId { get; set; }
    }

    /// <summary>
    /// Get reviews by product id query handler
    /// </summary>
    public class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, List<ReviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetReviewsByProductIdQueryHandler> _logger;

        public GetReviewsByProductIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetReviewsByProductIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ReviewResponse>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetReviewsByProductIdQuery for product: {ProductId}", request.ProductId);

            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return reviews
                .Where(r => r.ProductId == request.ProductId)
                .Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    VendorId = r.VendorId,
                    CustomerId = r.CustomerId,
                    RatingValue = r.RatingValue,
                    Title = r.Title,
                    Comment = r.Comment,
                    ImageUrls = r.Images.Select(i => i.ImageUrl).ToList(),
                    HelpfulCount = r.HelpfulCount,
                    IsVerifiedPurchase = r.IsVerifiedPurchase,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .ToList();
        }
    }
}



