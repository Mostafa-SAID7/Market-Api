using Market.API.Models.Entities;

namespace Market.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for review operations
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Get all reviews
        /// </summary>
        Task<IEnumerable<Review>> GetAllReviewsAsync();

        /// <summary>
        /// Get review by ID
        /// </summary>
        Task<Review?> GetReviewByIdAsync(string id);

        /// <summary>
        /// Get reviews by product
        /// </summary>
        Task<IEnumerable<Review>> GetReviewsByProductAsync(string productId);

        /// <summary>
        /// Get reviews by vendor
        /// </summary>
        Task<IEnumerable<Review>> GetReviewsByVendorAsync(string vendorId);

        /// <summary>
        /// Get reviews by customer
        /// </summary>
        Task<IEnumerable<Review>> GetReviewsByCustomerAsync(string customerId);

        /// <summary>
        /// Get verified purchase reviews only
        /// </summary>
        Task<IEnumerable<Review>> GetVerifiedReviewsAsync(string productId);

        /// <summary>
        /// Get reviews with rating
        /// </summary>
        Task<IEnumerable<Review>> GetReviewsByRatingAsync(string productId, int rating);

        /// <summary>
        /// Get top helpful reviews
        /// </summary>
        Task<IEnumerable<Review>> GetTopHelpfulReviewsAsync(string productId, int count = 10);

        /// <summary>
        /// Get average rating for product
        /// </summary>
        Task<double> GetAverageRatingAsync(string productId);

        /// <summary>
        /// Get rating distribution for product
        /// </summary>
        Task<Dictionary<int, int>> GetRatingDistributionAsync(string productId);

        /// <summary>
        /// Create a new review
        /// </summary>
        Task<Review> CreateReviewAsync(Review review);

        /// <summary>
        /// Update an existing review
        /// </summary>
        Task<Review> UpdateReviewAsync(string id, Review review);

        /// <summary>
        /// Mark review as helpful
        /// </summary>
        Task<Review> MarkHelpfulAsync(string id);

        /// <summary>
        /// Delete a review
        /// </summary>
        Task DeleteReviewAsync(string id);
    }
}
