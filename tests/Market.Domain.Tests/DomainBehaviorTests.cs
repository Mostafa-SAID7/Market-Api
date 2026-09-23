using Xunit;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Domain.ValueObjects;

namespace Market.Domain.Tests;

public class DomainBehaviorTests
{
    [Fact]
    public void Rating_Create_AcceptsInclusiveBoundaries()
    {
        Assert.Equal(1, Rating.Create(1).Value);
        Assert.Equal(5, Rating.Create(5).Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Rating_Create_RejectsValuesOutsideRange(int value)
    {
        Assert.Throws<ArgumentException>(() => Rating.Create(value));
    }

    [Fact]
    public void Money_Operations_RejectNegativeAndMixedCurrencyValues()
    {
        Assert.Throws<ArgumentException>(() => Money.Create(-0.01m));
        var usd = Money.Create(10m, "usd");
        Assert.Throws<InvalidOperationException>(() => usd.Add(Money.Create(1m, "EUR")));
        Assert.Equal(15m, usd.Add(Money.Create(5m)).Amount);
    }

    [Fact]
    public void Cart_AddItem_MergesSameProductRegardlessOfDuplicatedVendorInput()
    {
        var cart = new Cart();
        cart.AddItem(new CartItem { ProductId = 7, VendorId = 3, Quantity = 1, Price = 4m });
        cart.AddItem(new CartItem { ProductId = 7, VendorId = 99, Quantity = 2, Price = 4m });

        var item = Assert.Single(cart.Items);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(12m, cart.SubTotal);
    }

    [Fact]
    public void Cart_UpdateItemQuantity_RemovesItemAtZero()
    {
        var cart = new Cart { Items = [new CartItem { ProductId = 7, VendorId = 3, Quantity = 2, Price = 4m }] };

        cart.UpdateItemQuantity(7, 0);

        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Product_IsInStock_RequiresPositiveQuantityAndActiveStatus()
    {
        var product = new Product { Quantity = 1, Status = ProductStatus.Active };
        Assert.True(product.IsInStock);
        product.Status = ProductStatus.Inactive;
        Assert.False(product.IsInStock);
        product.Status = ProductStatus.Active;
        product.Quantity = 0;
        Assert.False(product.IsInStock);
    }

    [Fact]
    public void Category_Create_NormalizesSlugAndRejectsBlankName()
    {
        Assert.Equal("home-garden", Category.Create(" Home & Garden ", "Useful category").Slug);
        Assert.Throws<ArgumentException>(() => Category.Create(" ", "Useful category"));
    }
}
