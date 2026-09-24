namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating Product entities for testing.
/// </summary>
public class ProductBuilder
{
    private int _id = 1;
    private string _name = "Test Product";
    private string _description = "A test product";
    private int _vendorId = 1;
    private int _categoryId = 1;
    private decimal _price = 29.99m;
    private decimal _discountPrice = 0m;
    private int _quantity = 100;
    private ProductStatus _status = ProductStatus.Active;
    private decimal _averageRating = 4.5m;
    private int _reviewCount = 0;
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates a ProductBuilder with default values.
    /// </summary>
    public static ProductBuilder Default() => new();

    /// <summary>
    /// Creates a ProductBuilder with a specific ID.
    /// </summary>
    public static ProductBuilder WithId(int id) => new() { _id = id };

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithVendorId(int vendorId)
    {
        _vendorId = vendorId;
        return this;
    }

    public ProductBuilder WithCategoryId(int categoryId)
    {
        _categoryId = categoryId;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithDiscountPrice(decimal discountPrice)
    {
        _discountPrice = discountPrice;
        return this;
    }

    public ProductBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public ProductBuilder WithStatus(ProductStatus status)
    {
        _status = status;
        return this;
    }

    public ProductBuilder WithAverageRating(decimal rating)
    {
        _averageRating = rating;
        return this;
    }

    public ProductBuilder WithReviewCount(int count)
    {
        _reviewCount = count;
        return this;
    }

    public ProductBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the Product entity.
    /// </summary>
    public Product Build()
    {
        return new Product
        {
            Id = _id,
            Name = _name,
            Description = _description,
            VendorId = _vendorId,
            CategoryId = _categoryId,
            Price = _price,
            DiscountPrice = _discountPrice,
            Quantity = _quantity,
            Status = _status,
            AverageRating = _averageRating,
            ReviewCount = _reviewCount,
            CreatedAt = _createdAt
        };
    }
}
