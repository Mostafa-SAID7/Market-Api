using Market.Tests.Common.Base;

namespace Market.Domain.Tests.Entities;

public class OrderTests : TestBase
{
    [Fact]
    public void CalculateTotal_WithMultipleItems_UsesItemSubtotalsAndCharges()
    {
        var order = new Order
        {
            ShippingCost = 4m,
            Tax = 2m,
            Items =
            [
                new OrderItem { Price = 10m, Quantity = 2 },
                new OrderItem { Price = 5m, Quantity = 3 }
            ]
        };

        order.CalculateTotal();

        Assert.Equal(35m, order.SubTotal);
        Assert.Equal(41m, order.TotalPrice);
    }

    [Fact]
    public void GenerateOrderNumber_ReturnsExpectedPrefixAndUniqueValues()
    {
        var first = Order.GenerateOrderNumber();
        var second = Order.GenerateOrderNumber();

        Assert.Matches("^ORD-\\d{8}-[A-F0-9]{8}$", first);
        Assert.NotEqual(first, second);
    }
}
