using Market.Tests.Common.Base;

namespace Market.Domain.Tests.Entities;

/// <summary>
/// Tests for Category entity domain logic.
/// </summary>
public class CategoryTests : TestBase
{
    [Fact]
    public void Create_NormalizesSlugFromInput()
    {
        var category = Category.Create(" Home & Garden ", "Useful category");

        Assert.Equal("home-garden", category.Slug);
    }

    [Fact]
    public void Create_WithBlankName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Category.Create(" ", "Useful category"));
    }

    [Fact]
    public void Create_WithValidName_Succeeds()
    {
        var category = Category.Create("Electronics", "Tech products");

        Assert.Equal("Electronics", category.Name);
        Assert.NotEmpty(category.Slug);
    }
}
