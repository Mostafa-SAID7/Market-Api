using Market.Tests.Common.Base;

namespace Market.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for Money value object operations and currency handling.
/// </summary>
public class MoneyTests : TestBase
{
    [Fact]
    public void Create_WithNegativeAmount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Money.Create(-0.01m));
    }

    [Fact]
    public void Create_WithZeroAmount_Succeeds()
    {
        var money = Money.Create(0m);
        Assert.Equal(0m, money.Amount);
    }

    [Fact]
    public void Add_WithSameCurrency_Returns_SumAmount()
    {
        var usd1 = Money.Create(10m, "USD");
        var usd2 = Money.Create(5m, "USD");

        var result = usd1.Add(usd2);

        Assert.Equal(15m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Add_WithDifferentCurrency_ThrowsInvalidOperationException()
    {
        var usd = Money.Create(10m, "USD");
        var eur = Money.Create(5m, "EUR");

        Assert.Throws<InvalidOperationException>(() => usd.Add(eur));
    }

    [Fact]
    public void Add_WithDefaultCurrency_UsesUSD()
    {
        var money1 = Money.Create(10m);
        var money2 = Money.Create(5m);

        var result = money1.Add(money2);

        Assert.Equal(15m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }
}
