using Market.API.Common;
using Market.API.Models.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// Product category entity
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        
        public string SlugValue { get; set; } = string.Empty; // Store slug as string for MongoDB
        
        public string? ParentCategoryId { get; set; } // For subcategories
        public List<string> SubCategoryIds { get; set; } = new();

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Create category with slug
        /// </summary>
        public static Category Create(string name, string description, string? imageUrl = null)
        {
            var slug = Slug.Create(name);
            return new Category
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                SlugValue = slug.Value
            };
        }

        /// <summary>
        /// Get slug as value object
        /// </summary>
        public Slug GetSlug() => new Slug(SlugValue);

        public void UpdateSlugFromName()
        {
            var slug = Slug.Create(Name);
            SlugValue = slug.Value;
        }

        /// <summary>
        /// Add subcategory
        /// </summary>
        public void AddSubCategory(string subCategoryId)
        {
            if (!SubCategoryIds.Contains(subCategoryId))
            {
                SubCategoryIds.Add(subCategoryId);
            }
        }

        /// <summary>
        /// Remove subcategory
        /// </summary>
        public void RemoveSubCategory(string subCategoryId)
        {
            SubCategoryIds.Remove(subCategoryId);
        }
    }
}
