using Market.Domain.Entities;

namespace Market.Application.Validators
{
    /// <summary>
    /// Validator for Category entity
    /// </summary>
    public class CategoryValidator : IValidator<Category>
    {
        public ValidationResult Validate(Category entity)
        {
            var result = new ValidationResult();

            // Name validation
            if (string.IsNullOrWhiteSpace(entity.Name))
                result.AddError(nameof(Category.Name), "Category name is required");
            else if (entity.Name.Length < 2)
                result.AddError(nameof(Category.Name), "Category name must be at least 2 characters");
            else if (entity.Name.Length > 100)
                result.AddError(nameof(Category.Name), "Category name cannot exceed 100 characters");

            // Description validation
            if (string.IsNullOrWhiteSpace(entity.Description))
                result.AddError(nameof(Category.Description), "Category description is required");
            else if (entity.Description.Length < 5)
                result.AddError(nameof(Category.Description), "Category description must be at least 5 characters");
            else if (entity.Description.Length > 1000)
                result.AddError(nameof(Category.Description), "Category description cannot exceed 1000 characters");

            // Slug validation
            if (string.IsNullOrWhiteSpace(entity.Slug))
                result.AddError(nameof(Category.Slug), "Category slug is required");
            else if (!IsValidSlug(entity.Slug))
                result.AddError(nameof(Category.Slug), "Category slug contains invalid characters");

            // DisplayOrder validation
            if (entity.DisplayOrder < 0)
                result.AddError(nameof(Category.DisplayOrder), "Display order cannot be negative");

            return result;
        }

        private bool IsValidSlug(string slug)
        {
            return !string.IsNullOrEmpty(slug) && 
                   System.Text.RegularExpressions.Regex.IsMatch(slug, @"^[a-z0-9]+(?:-[a-z0-9]+)*$");
        }
    }
}


