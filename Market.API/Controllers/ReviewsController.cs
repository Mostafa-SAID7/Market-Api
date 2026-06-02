using Market.API.Models.Entities;
using Market.API.Services.Interfaces;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        /// <summary>
        /// Get all reviews
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        /// <summary>
        /// Get review by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        /// <summary>
        /// Get reviews by product
        /// </summary>
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(string productId)
        {
            var reviews = await _reviewService.GetReviewsByProductAsync(productId);
            return Ok(reviews);
        }

        /// <summary>
        /// Get reviews by vendor
        /// </summary>
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetByVendor(string vendorId)
        {
            var reviews = await _reviewService.GetReviewsByVendorAsync(vendorId);
            return Ok(reviews);
        }

        /// <summary>
        /// Get reviews by customer
        /// </summary>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            var reviews = await _reviewService.GetReviewsByCustomerAsync(customerId);
            return Ok(reviews);
        }

        /// <summary>
        /// Get verified reviews for product
        /// </summary>
        [HttpGet("product/{productId}/verified")]
        public async Task<IActionResult> GetVerified(string productId)
        {
            var reviews = await _reviewService.GetVerifiedReviewsAsync(productId);
            return Ok(reviews);
        }

        /// <summary>
        /// Get reviews by rating
        /// </summary>
        [HttpGet("product/{productId}/rating/{rating}")]
        public async Task<IActionResult> GetByRating(string productId, int rating)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByRatingAsync(productId, rating);
                return Ok(reviews);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid rating: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get top helpful reviews
        /// </summary>
        [HttpGet("product/{productId}/helpful")]
        public async Task<IActionResult> GetTopHelpful(string productId, [FromQuery] int count = 10)
        {
            var reviews = await _reviewService.GetTopHelpfulReviewsAsync(productId, count);
            return Ok(reviews);
        }

        /// <summary>
        /// Get average rating for product
        /// </summary>
        [HttpGet("product/{productId}/average-rating")]
        public async Task<IActionResult> GetAverageRating(string productId)
        {
            var average = await _reviewService.GetAverageRatingAsync(productId);
            return Ok(new { averageRating = average });
        }

        /// <summary>
        /// Get rating distribution for product
        /// </summary>
        [HttpGet("product/{productId}/rating-distribution")]
        public async Task<IActionResult> GetRatingDistribution(string productId)
        {
            var distribution = await _reviewService.GetRatingDistributionAsync(productId);
            return Ok(distribution);
        }

        /// <summary>
        /// Create a new review
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdReview = await _reviewService.CreateReviewAsync(review);
                return CreatedAtAction(nameof(Get), new { id = createdReview.Id }, createdReview);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid review data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing review
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedReview = await _reviewService.UpdateReviewAsync(id, review);
                return Ok(updatedReview);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Review not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid review data: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mark review as helpful
        /// </summary>
        [HttpPut("{id}/helpful")]
        public async Task<IActionResult> MarkHelpful(string id)
        {
            try
            {
                var review = await _reviewService.MarkHelpfulAsync(id);
                return Ok(new { success = true, review });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Review not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a review
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(id);
                return Ok(new { success = true, message = "Review deleted" });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Review not found: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
