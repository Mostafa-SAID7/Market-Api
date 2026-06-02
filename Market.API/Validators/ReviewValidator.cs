using Market.API.Models.Entities;

namespace Market.API.Validators
{
    /// <summary>
    /// Validator for Review entity
    /// </summary>
    public class ReviewValidator : IValidator<Review>
    {
        public ValidationResult Validate(Review entity)
        {
            var result = new ValidationResult();

            // ProductId validation
            if (string.IsNullOrWhiteSpace(entity.ProductId))
                result.AddError(nameof(Review.ProductId), "Product ID is required");

            // CustomerId validation
            if (string.IsNullOrWhiteSpace(entity.CustomerId))
                result.AddError(nameof(Review.CustomerId), "Customer ID is required");

            // VendorId validation
            if (string.IsNullOrWhiteSpace(entity.VendorId))
                result.AddError(nameof(Review.VendorId), "Vendor ID is required");

            // RatingValue validation
            if (entity.RatingValue < 1 || entity.RatingValue > 5)
                result.AddError(nameof(Review.RatingValue), "Rating value must be between 1 and 5");

            // Title validation
            if (string.IsNullOrWhiteSpace(entity.Title))
                result.AddError(nameof(Review.Title), "Review title is required");
            else if (entity.Title.Length < 3)
                result.AddError(nameof(Review.Title), "Review title must be at least 3 characters");
            else if (entity.Title.Length > 200)
                result.AddError(nameof(Review.Title), "Review title cannot exceed 200 characters");

            // Comment validation
            if (string.IsNullOrWhiteSpace(entity.Comment))
                result.AddError(nameof(Review.Comment), "Review comment is required");
            else if (entity.Comment.Length < 10)
                result.AddError(nameof(Review.Comment), "Review comment must be at least 10 characters");
            else if (entity.Comment.Length > 5000)
                result.AddError(nameof(Review.Comment), "Review comment cannot exceed 5000 characters");

            // HelpfulCount validation
            if (entity.HelpfulCount < 0)
                result.AddError(nameof(Review.HelpfulCount), "Helpful count cannot be negative");

            // ImageUrls validation
            if (entity.ImageUrls != null && entity.ImageUrls.Count > 0)
            {
                if (entity.ImageUrls.Count > 10)
                    result.AddError(nameof(Review.ImageUrls), "Cannot upload more than 10 images");

                foreach (var url in entity.ImageUrls)
                {
                    if (!IsValidUrl(url))
                        result.AddError(nameof(Review.ImageUrls), "Image URL format is invalid");
                }
            }

            return result;
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
