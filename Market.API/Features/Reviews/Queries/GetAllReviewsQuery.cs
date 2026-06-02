using MediatR;
using Market.API.Data.UnitOfWork;

namespace Market.API.Features.Reviews.Queries
{
    /// <summary>
    /// Get all reviews query
    /// </summary>
    public class GetAllReviewsQuery : IRequest<List<ReviewResponse>>
    {
    }

    /// <summary>
    /// Get all reviews query handler
    /// </summary>
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllReviewsQueryHandler> _logger;

        public GetAllReviewsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllReviewsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<ReviewResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllReviewsQuery");

            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return reviews.Select(r => new ReviewResponse
            {
                Id = r.Id,
                ProductId = r.ProductId,
                VendorId = r.VendorId,
                CustomerId = r.CustomerId,
                RatingValue = r.RatingValue,
                Title = r.Title,
                Comment = r.Comment,
                ImageUrls = r.ImageUrls,
                HelpfulCount = r.HelpfulCount,
                IsVerifiedPurchase = r.IsVerifiedPurchase,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();
        }
    }
}
