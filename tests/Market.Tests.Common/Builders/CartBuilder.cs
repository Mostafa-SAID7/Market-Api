namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating Cart entities for testing.
/// </summary>
public class CartBuilder
{
    private int _id = 1;
    private int _userId = 1;
    private List<CartItem> _items = [];
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime? _modifiedAt;

    /// <summary>
    /// Creates a CartBuilder with default values.
    /// </summary>
    public static CartBuilder Default() => new();

    /// <summary>
    /// Creates a CartBuilder with a specific user ID.
    /// </summary>
    public static CartBuilder ForUser(int userId) => new() { _userId = userId };

    public CartBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CartBuilder WithUserId(int userId)
    {
        _userId = userId;
        return this;
    }

    public CartBuilder WithItems(IEnumerable<CartItem> items)
    {
        _items = items.ToList();
        return this;
    }

    public CartBuilder AddItem(CartItem item)
    {
        _items.Add(item);
        return this;
    }

    public CartBuilder AddItem(int productId, int vendorId, int quantity, decimal price)
    {
        _items.Add(new CartItem
        {
            ProductId = productId,
            VendorId = vendorId,
            Quantity = quantity,
            Price = price
        });
        return this;
    }

    public CartBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public CartBuilder WithModifiedAt(DateTime? modifiedAt)
    {
        _modifiedAt = modifiedAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the Cart entity.
    /// </summary>
    public Cart Build()
    {
        return new Cart
        {
            Id = _id,
            UserId = _userId,
            Items = _items,
            CreatedAt = _createdAt,
            UpdatedAt = _modifiedAt
        };
    }
}
