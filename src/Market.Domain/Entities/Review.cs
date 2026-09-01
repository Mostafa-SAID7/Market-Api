using Market.Domain.Common;
using Market.Domain.ValueObjects;

namespace Market.Domain.Entities
{
    /// <summary>
    /// Review/Rating entity for products
    /// </summary>
    public class Review : BaseEntity
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        
        public int RatingValue { get; set; } // 1-5
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public int HelpfulCount { get; set; } = 0;
        public bool IsVerifiedPurchase { get; set; } = false;

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Vendor Vendor { get; set; } = null!;
        public User Customer { get; set; } = null!;
        public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();

        /// <summary>
        /// Get rating as value object
        /// </summary>
        public Rating GetRating() => Rating.Create(RatingValue);

        /// <summary>
        /// Set rating from value object
        /// </summary>
        public void SetRating(Rating rating)
        {
            RatingValue = rating.Value;
        }
    }

    /// <summary>
    /// ReviewImage - image in a review
    /// </summary>
    public class ReviewImage
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation
        public Review Review { get; set; } = null!;
    }
}


