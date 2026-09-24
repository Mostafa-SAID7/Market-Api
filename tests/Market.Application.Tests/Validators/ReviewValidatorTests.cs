namespace Market.Application.Tests.Validators;

public class ReviewValidatorTests
{
    private readonly ReviewValidator _validator = new();

    [Fact]
    public void Validate_WithValidReviewAndHttpsImage_ReturnsValidResult()
    {
        var review = new Review
        {
            ProductId = 1, CustomerId = 2, VendorId = 3, RatingValue = 5,
            Title = "Excellent", Comment = "This product exceeded my expectations.",
            Images = [new ReviewImage { ImageUrl = "https://example.test/review.jpg" }]
        };

        var result = _validator.Validate(review);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithInvalidRatingAndImageUrl_ReturnsExpectedErrors()
    {
        var review = new Review
        {
            ProductId = 1, CustomerId = 2, VendorId = 3, RatingValue = 6,
            Title = "Excellent", Comment = "This product exceeded my expectations.",
            Images = [new ReviewImage { ImageUrl = "javascript:alert(1)" }]
        };

        var result = _validator.Validate(review);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Field == nameof(Review.RatingValue));
        Assert.Contains(result.Errors, error => error.Field == nameof(Review.Images));
    }
}
