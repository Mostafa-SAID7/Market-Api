using Market.API.Common;
using Market.API.Models.Enums;
using Market.API.Models.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// Product entity for e-commerce platform
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Product : BaseEntity
    {
        public string VendorId { get; set; } = string.Empty; // Reference to Vendor
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; } = 0;

        public string Category { get; set; } = string.Empty;
        public string? SubCategory { get; set; }
        
        public List<string> TagNames { get; set; } = new(); // Store tag names as strings for MongoDB
        public string? SKU { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.Active;
        
        public double AverageRating { get; set; } = 0.0;
        public int ReviewCount { get; set; } = 0;

        /// <summary>
        /// Add tags to the product
        /// </summary>
        public void AddTags(params string[] tagNames)
        {
            foreach (var name in tagNames)
            {
                var tag = Tag.Create(name);
                if (!TagNames.Contains(tag.Name, StringComparer.OrdinalIgnoreCase))
                {
                    TagNames.Add(tag.Name);
                }
            }
        }

        /// <summary>
        /// Remove a tag from the product
        /// </summary>
        public void RemoveTag(string tagName)
        {
            TagNames.RemoveAll(t => t.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get calculated profit after commission
        /// </summary>
        public decimal CalculateProfit(decimal commissionRate)
        {
            return Price * (1 - commissionRate);
        }

        /// <summary>
        /// Check if product is in stock
        /// </summary>
        public bool IsInStock => Quantity > 0 && Status == ProductStatus.Active;
    }
}
