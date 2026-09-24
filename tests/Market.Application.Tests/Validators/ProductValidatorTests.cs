using Market.Tests.Common.Base;
using Market.Tests.Common.Builders;

namespace Market.Application.Tests.Validators;

/// <summary>
/// Tests for ProductValidator business rule validation.
/// </summary>
public class ProductValidatorTests : ValidatorTestBase
{
    [Fact]
    public void Validate_WithValidProduct_Passes()
    {
        var validator = new ProductValidator();
        var product = ProductBuilder.Default().Build();

        var result = validator.Validate(product);

        AssertIsValid(result);
    }

    [Fact]
    public void Validate_WithInvalidData_FailsMultipleValidations()
    {
        var validator = new ProductValidator();
        var product = new Product
        {
            Name = " ",
            Description = "short",
            VendorId = 0,
            CategoryId = 0,
            Price = 0m,
            DiscountPrice = 0m,
            Quantity = -1,
            AverageRating = 5.01m
        };

        var result = validator.Validate(product);

        AssertIsInvalid(result);
        AssertHasError(result.Errors, nameof(Product.Price));
        AssertHasError(result.Errors, nameof(Product.AverageRating));
    }
}
