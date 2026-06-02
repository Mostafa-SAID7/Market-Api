using Market.API.Models.Entities;

namespace Market.API.Data.Interfaces
{
    /// <summary>
    /// Repository interface for review-specific operations
    /// </summary>
    public interface IReviewRepository : IRepository<Review>
    {
        /// <summary>
        /// Get reviews by product ID
        /// </summary>
        Task<IEnumerable<Review>> GetByProductIdAsync(string productId);

        /// <summary>
        /// Get reviews by vendor ID
        /// </summary>
        Task<IEnumerable<Review>> GetByVendorIdAsync(string vendorId);

        /// <summary>
        /// Get reviews by customer ID
        /// </summary>
        Task<IEnumerable<Review>> GetByCustomerIdAsync(string customerId);

        /// <summary>
        /// Get verified purchase reviews
        /// </summary>
        Task<IEnumerable<Review>> GetVerifiedReviewsAsync(string productId);

        /// <summary>
        /// Get reviews by rating
        /// </summary>
        Task<IEnumerable<Review>> GetByRatingAsync(string productId, int rating);

        /// <summary>
        /// Get top helpful reviews
        /// </summary>
        Task<IEnumerable<Review>> GetTopHelpfulAsync(string productId, int count = 10);

        /// <summary>
        /// Check if customer reviewed product
        /// </summary>
        Task<bool> CustomerReviewedProductAsync(string productId, string customerId);
    }
}
