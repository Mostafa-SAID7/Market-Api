using Market.Tests.Common.Base;

namespace Market.Application.Tests.Validators;

/// <summary>
/// Tests for VendorValidator commission rate and business constraints.
/// </summary>
public class VendorValidatorTests : ValidatorTestBase
{
    [Theory]
    [InlineData(0.00)]
    [InlineData(0.15)]
    [InlineData(0.50)]
    public void Validate_WithValidCommissionRate_Passes(decimal rate)
    {
        var validator = new VendorValidator();
        var vendor = ValidVendor(rate);

        var result = validator.Validate(vendor);

        AssertIsValid(result);
    }

    [Fact]
    public void Validate_WithCommissionRateAboveConstraint_Fails()
    {
        var validator = new VendorValidator();
        var vendor = ValidVendor(0.51m);

        var result = validator.Validate(vendor);

        AssertIsInvalid(result);
        AssertHasError(result.Errors, nameof(Vendor.CommissionRate));
    }

    private static Vendor ValidVendor(decimal rate) => new()
    {
        UserId = 1,
        StoreName = "Good Store",
        StoreDescription = "A good store description.",
        CommissionRate = rate,
        AverageRating = 5m
    };
}
