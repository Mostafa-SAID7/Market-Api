using Market.API.Data.UnitOfWork;
using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Services
{
    /// <summary>
    /// Service for handling review business logic
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IUnitOfWork unitOfWork, ILogger<ReviewService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            _logger.LogInformation("Fetching all reviews");
            return await _unitOfWork.Reviews.GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<Review?> GetReviewByIdAsync(string id)
        {
            _logger.LogInformation("Fetching review with ID: {ReviewId}", id);
            return await _unitOfWork.Reviews.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetReviewsByProductAsync(string productId)
        {
            _logger.LogInformation("Fetching reviews for product: {ProductId}", productId);
            return await _unitOfWork.Reviews.GetByProductIdAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetReviewsByVendorAsync(string vendorId)
        {
            _logger.LogInformation("Fetching reviews for vendor: {VendorId}", vendorId);
            return await _unitOfWork.Reviews.GetByVendorIdAsync(vendorId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetReviewsByCustomerAsync(string customerId)
        {
            _logger.LogInformation("Fetching reviews by customer: {CustomerId}", customerId);
            return await _unitOfWork.Reviews.GetByCustomerIdAsync(customerId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetVerifiedReviewsAsync(string productId)
        {
            _logger.LogInformation("Fetching verified reviews for product: {ProductId}", productId);
            return await _unitOfWork.Reviews.GetVerifiedReviewsAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetReviewsByRatingAsync(string productId, int rating)
        {
            _logger.LogInformation("Fetching reviews with rating {Rating} for product: {ProductId}", rating, productId);

            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));

            return await _unitOfWork.Reviews.GetByRatingAsync(productId, rating);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Review>> GetTopHelpfulReviewsAsync(string productId, int count = 10)
        {
            _logger.LogInformation("Fetching top {Count} helpful reviews for product: {ProductId}", count, productId);
            return await _unitOfWork.Reviews.GetTopHelpfulAsync(productId, count);
        }

        /// <inheritdoc/>
        public async Task<double> GetAverageRatingAsync(string productId)
        {
            _logger.LogInformation("Calculating average rating for product: {ProductId}", productId);

            var reviews = await _unitOfWork.Reviews.GetByProductIdAsync(productId);
            if (!reviews.Any())
                return 0.0;

            return Math.Round(reviews.Average(r => r.RatingValue), 2);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<int, int>> GetRatingDistributionAsync(string productId)
        {
            _logger.LogInformation("Getting rating distribution for product: {ProductId}", productId);

            var reviews = await _unitOfWork.Reviews.GetByProductIdAsync(productId);
            var distribution = new Dictionary<int, int>
            {
                { 1, reviews.Count(r => r.RatingValue == 1) },
                { 2, reviews.Count(r => r.RatingValue == 2) },
                { 3, reviews.Count(r => r.RatingValue == 3) },
                { 4, reviews.Count(r => r.RatingValue == 4) },
                { 5, reviews.Count(r => r.RatingValue == 5) }
            };

            return distribution;
        }

        /// <inheritdoc/>
        public async Task<Review> CreateReviewAsync(Review review)
        {
            _logger.LogInformation("Creating new review for product: {ProductId}", review.ProductId);

            if (string.IsNullOrWhiteSpace(review.ProductId))
                throw new ArgumentException("Product ID cannot be empty", nameof(review.ProductId));

            if (string.IsNullOrWhiteSpace(review.CustomerId))
                throw new ArgumentException("Customer ID cannot be empty", nameof(review.CustomerId));

            if (review.RatingValue < 1 || review.RatingValue > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(review.RatingValue));

            // Check if customer already reviewed this product
            var alreadyReviewed = await _unitOfWork.Reviews.CustomerReviewedProductAsync(review.ProductId, review.CustomerId);
            if (alreadyReviewed)
            {
                _logger.LogWarning("Customer already reviewed this product - Product: {ProductId}, Customer: {CustomerId}", review.ProductId, review.CustomerId);
                throw new InvalidOperationException("Customer has already reviewed this product");
            }

            await _unitOfWork.Reviews.CreateAsync(review);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Review created successfully with ID: {ReviewId}", review.Id);
            return review;
        }

        /// <inheritdoc/>
        public async Task<Review> UpdateReviewAsync(string id, Review review)
        {
            _logger.LogInformation("Updating review with ID: {ReviewId}", id);

            var existingReview = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (existingReview == null)
            {
                _logger.LogWarning("Review not found for update: {ReviewId}", id);
                throw new KeyNotFoundException($"Review with ID {id} not found");
            }

            if (review.RatingValue < 1 || review.RatingValue > 5)
                throw new ArgumentException("Rating must be between 1 and 5", nameof(review.RatingValue));

            review.Id = id;
            review.ProductId = existingReview.ProductId; // Prevent product change
            review.CustomerId = existingReview.CustomerId; // Prevent customer change
            await _unitOfWork.Reviews.UpdateAsync(id, review);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Review updated successfully: {ReviewId}", id);
            return review;
        }

        /// <inheritdoc/>
        public async Task<Review> MarkHelpfulAsync(string id)
        {
            _logger.LogInformation("Marking review as helpful - ID: {ReviewId}", id);

            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
            {
                _logger.LogWarning("Review not found: {ReviewId}", id);
                throw new KeyNotFoundException($"Review with ID {id} not found");
            }

            review.HelpfulCount++;
            await _unitOfWork.Reviews.UpdateAsync(id, review);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Review marked as helpful: {ReviewId}", id);
            return review;
        }

        /// <inheritdoc/>
        public async Task DeleteReviewAsync(string id)
        {
            _logger.LogInformation("Deleting review with ID: {ReviewId}", id);

            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
            {
                _logger.LogWarning("Review not found for deletion: {ReviewId}", id);
                throw new KeyNotFoundException($"Review with ID {id} not found");
            }

            await _unitOfWork.Reviews.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Review deleted successfully: {ReviewId}", id);
        }
    }
}
