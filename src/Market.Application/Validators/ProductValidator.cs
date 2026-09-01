using Market.Domain.Entities;

namespace Market.Application.Validators
{
    /// <summary>
    /// Validator for Product entity
    /// </summary>
    public class ProductValidator : IValidator<Product>
    {
        public ValidationResult Validate(Product entity)
        {
            var result = new ValidationResult();

            // Name validation
            if (string.IsNullOrWhiteSpace(entity.Name))
                result.AddError(nameof(Product.Name), "Product name is required");
            else if (entity.Name.Length < 3)
                result.AddError(nameof(Product.Name), "Product name must be at least 3 characters");
            else if (entity.Name.Length > 500)
                result.AddError(nameof(Product.Name), "Product name cannot exceed 500 characters");

            // Description validation
            if (string.IsNullOrWhiteSpace(entity.Description))
                result.AddError(nameof(Product.Description), "Product description is required");
            else if (entity.Description.Length < 10)
                result.AddError(nameof(Product.Description), "Product description must be at least 10 characters");
            else if (entity.Description.Length > 5000)
                result.AddError(nameof(Product.Description), "Product description cannot exceed 5000 characters");

            // VendorId validation
            if (entity.VendorId <= 0)
                result.AddError(nameof(Product.VendorId), "Vendor ID is required");

            // Category validation
            if (entity.CategoryId <= 0)
                result.AddError(nameof(Product.CategoryId), "Category is required");

            // Price validation
            if (entity.Price <= 0)
                result.AddError(nameof(Product.Price), "Price must be greater than 0");
            else if (entity.Price > 1000000)
                result.AddError(nameof(Product.Price), "Price cannot exceed 1,000,000");

            // Discount price validation
            if (entity.DiscountPrice.HasValue)
            {
                if (entity.DiscountPrice <= 0)
                    result.AddError(nameof(Product.DiscountPrice), "Discount price must be greater than 0");
                else if (entity.DiscountPrice >= entity.Price)
                    result.AddError(nameof(Product.DiscountPrice), "Discount price must be less than regular price");
            }

            // Quantity validation
            if (entity.Quantity < 0)
                result.AddError(nameof(Product.Quantity), "Quantity cannot be negative");

            // Rating validation
            if (entity.AverageRating < 0 || entity.AverageRating > 5)
                result.AddError(nameof(Product.AverageRating), "Average rating must be between 0 and 5");

            return result;
        }
    }
}


