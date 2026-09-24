using Market.Tests.Common.Base;

namespace Market.Application.Tests.Validators;

/// <summary>
/// Tests for ReviewValidator rating bounds and security checks.
/// </summary>
public class ReviewValidatorTests : ValidatorTestBase
{
    [Fact]
    public void Validate_WithInvalidRatingAndUnsafeUrl_Fails()
    {
        var validator = new ReviewValidator();
        var review = new Review
        {
            ProductId = 1,
            CustomerId = 1,
            VendorId = 1,
            RatingValue = 6,
            Title = "Bad",
            Comment = "Long enough comment",
            Images = [new ReviewImage { ImageUrl = "file:///etc/passwd" }]
        };

        var result = validator.Validate(review);

        AssertIsInvalid(result);
        AssertHasError(result.Errors, nameof(Review.RatingValue));
        AssertHasError(result.Errors, nameof(Review.Images));
    }

    [Fact]
    public void Validate_WithValidReview_Passes()
    {
        var validator = new ReviewValidator();
        var review = new Review
        {
            ProductId = 1,
            CustomerId = 1,
            VendorId = 1,
            RatingValue = 5,
            Title = "Great product",
            Comment = "This product exceeded my expectations",
            Images = [new ReviewImage { ImageUrl = "https://example.com/image.jpg" }]
        };

        var result = validator.Validate(review);

        AssertIsValid(result);
    }
}
