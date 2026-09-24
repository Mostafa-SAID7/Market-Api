namespace Market.Application.Tests.Validators;

public class VendorValidatorTests
{
    private readonly VendorValidator _validator = new();

    [Theory]
    [InlineData(0.00)]
    [InlineData(0.15)]
    [InlineData(0.50)]
    public void Validate_WithCommissionRateInSupportedRange_ReturnsValidResult(double rate)
    {
        var vendor = new Vendor
        {
            UserId = 1, StoreName = "Market Store", StoreDescription = "A dependable online market store.",
            CommissionRate = (decimal)rate, AverageRating = 0, TotalReviews = 0
        };

        var result = _validator.Validate(vendor);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithInvalidBusinessValues_ReturnsErrorsForAffectedFields()
    {
        var vendor = new Vendor
        {
            UserId = 0, StoreName = "x", StoreDescription = "short", CommissionRate = .51m,
            PhoneNumber = "invalid", AverageRating = 6, TotalReviews = -1
        };

        var result = _validator.Validate(vendor);

        Assert.False(result.IsValid);
        Assert.Equal(
            [nameof(Vendor.UserId), nameof(Vendor.StoreName), nameof(Vendor.StoreDescription), nameof(Vendor.CommissionRate),
             nameof(Vendor.PhoneNumber), nameof(Vendor.AverageRating), nameof(Vendor.TotalReviews)],
            result.Errors.Select(error => error.Field));
    }
}
