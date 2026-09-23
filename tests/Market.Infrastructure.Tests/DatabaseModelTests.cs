using Xunit;
using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Market.Infrastructure.Tests;

public class DatabaseModelTests
{
    private static MarketDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MarketDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MarketModelTests;Trusted_Connection=True")
            .Options;
        return new MarketDbContext(options);
    }

    [Fact]
    public void Model_EnforcesActiveReviewUniquenessAndRatingRange()
    {
        using var context = CreateContext();
        var review = context.Model.FindEntityType(typeof(Review))!;
        var index = Assert.Single(review.GetIndexes(), i => i.Properties.Select(p => p.Name).SequenceEqual([nameof(Review.CustomerId), nameof(Review.ProductId)]));
        Assert.True(index.IsUnique);
        Assert.Equal("[IsDeleted] = 0", index.GetFilter());
    }

    [Fact]
    public void Model_EnforcesVendorUserAndCartItemUniqueness()
    {
        using var context = CreateContext();
        var vendor = context.Model.FindEntityType(typeof(Vendor))!;
        Assert.Contains(vendor.GetIndexes(), i => i.IsUnique && i.Properties.Single().Name == nameof(Vendor.UserId) && i.GetFilter() == "[IsDeleted] = 0");

        var cartItem = context.Model.FindEntityType(typeof(CartItem))!;
        Assert.Contains(cartItem.GetIndexes(), i => i.IsUnique && i.Properties.Select(p => p.Name).SequenceEqual([nameof(CartItem.CartId), nameof(CartItem.ProductId)]));
    }
}
