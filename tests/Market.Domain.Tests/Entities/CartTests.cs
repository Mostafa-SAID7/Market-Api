using Market.Tests.Common.Base;
using Market.Tests.Common.Builders;

namespace Market.Domain.Tests.Entities;

/// <summary>
/// Tests for Cart entity domain logic and invariants.
/// </summary>
public class CartTests : TestBase
{
    [Fact]
    public void AddItem_WithNewProduct_IncreasesItemCount()
    {
        var cart = new Cart();
        var item = CartItemBuilder.Default().WithProductId(1).Build();

        cart.AddItem(item);

        Assert.Single(cart.Items);
        Assert.Equal(1, cart.Items.First().ProductId);
    }

    [Fact]
    public void AddItem_WithDuplicateProduct_MergesQuantity()
    {
        var cart = new Cart();
        cart.AddItem(new CartItem { ProductId = 7, VendorId = 3, Quantity = 1, Price = 4m });
        cart.AddItem(new CartItem { ProductId = 7, VendorId = 99, Quantity = 2, Price = 4m });

        var item = Assert.Single(cart.Items);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(12m, cart.SubTotal);
    }

    [Fact]
    public void UpdateItemQuantity_WithZero_RemovesItem()
    {
        var cart = new Cart
        {
            Items = [new CartItem { ProductId = 7, VendorId = 3, Quantity = 2, Price = 4m }]
        };

        cart.UpdateItemQuantity(7, 0);

        Assert.Empty(cart.Items);
    }

    [Fact]
    public void UpdateItemQuantity_WithPositiveValue_UpdatesQuantity()
    {
        var item = new CartItem { ProductId = 7, VendorId = 3, Quantity = 2, Price = 4m };
        var cart = new Cart { Items = [item] };

        cart.UpdateItemQuantity(7, 5);

        Assert.Single(cart.Items);
        Assert.Equal(5, cart.Items.First().Quantity);
    }

    [Fact]
    public void SubTotal_CalculatesCorrectly()
    {
        var cart = CartBuilder.Default()
            .AddItem(1, 1, 2, 10m)  // 2 * 10 = 20
            .AddItem(2, 1, 3, 5m)   // 3 * 5 = 15
            .Build();

        // SubTotal should be 20 + 15 = 35
        Assert.Equal(35m, cart.SubTotal);
    }
}
