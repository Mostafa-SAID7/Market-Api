using MediatR;
using Market.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Reviews.Commands
{
    /// <summary>
    /// Create review command
    /// </summary>
    public class CreateReviewCommand : IRequest<ReviewResponse>
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        public int RatingValue { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
    }

    /// <summary>
    /// Create review command handler
    /// </summary>
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateReviewCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ReviewResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateReviewCommand for product: {ProductId} by customer: {CustomerId}", 
                request.ProductId, request.CustomerId);

            // Check if customer already reviewed this product
            var existingReview = await _unitOfWork.Reviews.CustomerReviewedProductAsync(
                request.ProductId, request.CustomerId, cancellationToken);
            
            if (existingReview)
            {
                throw new InvalidOperationException(
                    $"Customer {request.CustomerId} has already reviewed product {request.ProductId}");
            }

            var review = new Review
            {
                ProductId = request.ProductId,
                VendorId = request.VendorId,
                CustomerId = request.CustomerId,
                RatingValue = request.RatingValue,
                Title = request.Title,
                Comment = request.Comment
            };

            // Add images as ReviewImage entities
            foreach (var imageUrl in request.ImageUrls)
            {
                review.Images.Add(new ReviewImage { ImageUrl = imageUrl });
            }

            await _unitOfWork.Reviews.CreateAsync(review, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new ReviewResponse
            {
                Id = review.Id,
                ProductId = review.ProductId,
                VendorId = review.VendorId,
                CustomerId = review.CustomerId,
                RatingValue = review.RatingValue,
                Title = review.Title,
                Comment = review.Comment,
                ImageUrls = review.Images.Select(i => i.ImageUrl).ToList(),
                HelpfulCount = review.HelpfulCount,
                IsVerifiedPurchase = review.IsVerifiedPurchase,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };
        }
    }
}



