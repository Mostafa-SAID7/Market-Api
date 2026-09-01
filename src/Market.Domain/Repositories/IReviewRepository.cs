using Market.Domain.Entities;

namespace Market.Domain.Repositories
{
    /// <summary>
    /// Repository interface for review-specific operations
    /// </summary>
    public interface IReviewRepository : IRepository<Review>
    {
        /// <summary>
        /// Get reviews by product ID
        /// </summary>
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get reviews by vendor ID
        /// </summary>
        Task<IEnumerable<Review>> GetByVendorIdAsync(int vendorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get reviews by customer ID
        /// </summary>
        Task<IEnumerable<Review>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get verified purchase reviews
        /// </summary>
        Task<IEnumerable<Review>> GetVerifiedReviewsAsync(int productId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get reviews by rating
        /// </summary>
        Task<IEnumerable<Review>> GetByRatingAsync(int productId, int rating, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get top helpful reviews
        /// </summary>
        Task<IEnumerable<Review>> GetTopHelpfulAsync(int productId, int count = 10, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if customer reviewed product
        /// </summary>
        Task<bool> CustomerReviewedProductAsync(int productId, int customerId, CancellationToken cancellationToken = default);
    }
}


