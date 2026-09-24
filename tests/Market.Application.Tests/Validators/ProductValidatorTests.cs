namespace Market.Application.Tests.Validators;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator = new();

    [Fact]
    public void Validate_WithValidProduct_ReturnsValidResult()
    {
        var product = new Product
        {
            Name = "Wireless keyboard", Description = "A reliable wireless keyboard.", VendorId = 1,
            CategoryId = 1, Price = 40m, Quantity = 0, AverageRating = 0
        };

        var result = _validator.Validate(product);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithInvalidValues_ReturnsErrorsForEachInvalidField()
    {
        var product = new Product
        {
            Name = "x", Description = "short", VendorId = 0, CategoryId = 0, Price = 0,
            DiscountPrice = 1m, Quantity = -1, AverageRating = 6m
        };

        var result = _validator.Validate(product);

        Assert.False(result.IsValid);
        Assert.Equal(
            [nameof(Product.Name), nameof(Product.Description), nameof(Product.VendorId), nameof(Product.CategoryId),
             nameof(Product.Price), nameof(Product.DiscountPrice), nameof(Product.Quantity), nameof(Product.AverageRating)],
            result.Errors.Select(error => error.Field));
    }
}
