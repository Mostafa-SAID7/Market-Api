using Market.Tests.Common.Base;
using Market.Tests.Common.Builders;

namespace Market.Domain.Tests.Entities;

/// <summary>
/// Tests for Product entity domain logic and state.
/// </summary>
public class ProductTests : TestBase
{
    [Fact]
    public void IsInStock_WithActiveStatusAndPositiveQuantity_ReturnsTrue()
    {
        var product = new Product
        {
            Quantity = 1,
            Status = ProductStatus.Active
        };

        Assert.True(product.IsInStock);
    }

    [Fact]
    public void IsInStock_WithInactiveStatus_ReturnsFalse()
    {
        var product = new Product
        {
            Quantity = 10,
            Status = ProductStatus.Inactive
        };

        Assert.False(product.IsInStock);
    }

    [Fact]
    public void IsInStock_WithZeroQuantity_ReturnsFalse()
    {
        var product = new Product
        {
            Quantity = 0,
            Status = ProductStatus.Active
        };

        Assert.False(product.IsInStock);
    }

    [Fact]
    public void IsInStock_WithNegativeQuantity_ReturnsFalse()
    {
        var product = new Product
        {
            Quantity = -5,
            Status = ProductStatus.Active
        };

        Assert.False(product.IsInStock);
    }
}
