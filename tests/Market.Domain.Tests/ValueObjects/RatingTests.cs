using Market.Tests.Common.Base;

namespace Market.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for Rating value object validation and behavior.
/// </summary>
public class RatingTests : TestBase
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void Create_WithValidBoundaryValues_Succeeds(int value)
    {
        var rating = Rating.Create(value);

        Assert.Equal(value, rating.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(10)]
    public void Create_WithInvalidValues_ThrowsArgumentException(int value)
    {
        Assert.Throws<ArgumentException>(() => Rating.Create(value));
    }
}
