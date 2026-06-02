using Market.API.Common;
using Market.API.Models.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// Review/Rating entity for products
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Review : BaseEntity
    {
        public string ProductId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty; // Reference to User
        
        public int RatingValue { get; set; } // 1-5
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public List<string> ImageUrls { get; set; } = new();

        public int HelpfulCount { get; set; } = 0;
        public bool IsVerifiedPurchase { get; set; } = false;

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
}

