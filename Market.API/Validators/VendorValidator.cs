using Market.API.Models.Entities;

namespace Market.API.Validators
{
    /// <summary>
    /// Validator for Vendor entity
    /// </summary>
    public class VendorValidator : IValidator<Vendor>
    {
        public ValidationResult Validate(Vendor entity)
        {
            var result = new ValidationResult();

            // UserId validation
            if (string.IsNullOrWhiteSpace(entity.UserId))
                result.AddError(nameof(Vendor.UserId), "User ID is required");

            // StoreName validation
            if (string.IsNullOrWhiteSpace(entity.StoreName))
                result.AddError(nameof(Vendor.StoreName), "Store name is required");
            else if (entity.StoreName.Length < 3)
                result.AddError(nameof(Vendor.StoreName), "Store name must be at least 3 characters");
            else if (entity.StoreName.Length > 200)
                result.AddError(nameof(Vendor.StoreName), "Store name cannot exceed 200 characters");

            // StoreDescription validation
            if (string.IsNullOrWhiteSpace(entity.StoreDescription))
                result.AddError(nameof(Vendor.StoreDescription), "Store description is required");
            else if (entity.StoreDescription.Length < 10)
                result.AddError(nameof(Vendor.StoreDescription), "Store description must be at least 10 characters");
            else if (entity.StoreDescription.Length > 2000)
                result.AddError(nameof(Vendor.StoreDescription), "Store description cannot exceed 2000 characters");

            // CommissionRate validation
            if (entity.CommissionRate < 0 || entity.CommissionRate > 1)
                result.AddError(nameof(Vendor.CommissionRate), "Commission rate must be between 0 and 1");

            // PhoneNumber validation
            if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(entity.PhoneNumber, @"^\+?[1-9]\d{1,14}$"))
                    result.AddError(nameof(Vendor.PhoneNumber), "Phone number format is invalid");
            }

            // AverageRating validation
            if (entity.AverageRating < 0 || entity.AverageRating > 5)
                result.AddError(nameof(Vendor.AverageRating), "Average rating must be between 0 and 5");

            // TotalReviews validation
            if (entity.TotalReviews < 0)
                result.AddError(nameof(Vendor.TotalReviews), "Total reviews cannot be negative");

            return result;
        }
    }
}
