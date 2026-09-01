using Market.Domain.Common;
using Market.Domain.ValueObjects;

namespace Market.Domain.Entities
{
    /// <summary>
    /// Product category entity with hierarchical support
    /// </summary>
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        
        public string Slug { get; set; } = string.Empty;
        
        public int? ParentCategoryId { get; set; } // For subcategories
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        public Category? Parent { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Create category with slug
        /// </summary>
        public static Category Create(string name, string description, string? imageUrl = null)
        {
            var slugVo = ValueObjects.Slug.Create(name);
            return new Category
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                Slug = slugVo.Value
            };
        }

        /// <summary>
        /// Get slug as value object
        /// </summary>
        public ValueObjects.Slug GetSlug() => new ValueObjects.Slug(Slug);

        public void UpdateSlugFromName()
        {
            var slugVo = ValueObjects.Slug.Create(Name);
            Slug = slugVo.Value;
        }
    }
}

