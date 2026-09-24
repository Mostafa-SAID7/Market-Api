namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating Category entities for testing.
/// </summary>
public class CategoryBuilder
{
    private int _id = 1;
    private string _name = "Test Category";
    private string _slug = "test-category";
    private int? _parentCategoryId;
    private List<Product> _products = [];
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates a CategoryBuilder with default values.
    /// </summary>
    public static CategoryBuilder Default() => new();

    /// <summary>
    /// Creates a CategoryBuilder with a specific name and auto-generated slug.
    /// </summary>
    public static CategoryBuilder Named(string name)
    {
        var slug = name.ToLower().Replace(" ", "-").Replace("&", "and");
        return new CategoryBuilder { _name = name, _slug = slug };
    }

    public CategoryBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CategoryBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CategoryBuilder WithSlug(string slug)
    {
        _slug = slug;
        return this;
    }

    public CategoryBuilder WithParentCategoryId(int? parentCategoryId)
    {
        _parentCategoryId = parentCategoryId;
        return this;
    }

    public CategoryBuilder WithProducts(IEnumerable<Product> products)
    {
        _products = products.ToList();
        return this;
    }

    public CategoryBuilder AddProduct(Product product)
    {
        _products.Add(product);
        return this;
    }

    public CategoryBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the Category entity.
    /// </summary>
    public Category Build()
    {
        return new Category
        {
            Id = _id,
            Name = _name,
            Slug = _slug,
            ParentCategoryId = _parentCategoryId,
            Products = _products,
            CreatedAt = _createdAt
        };
    }
}
