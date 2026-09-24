namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating CartItem entities for testing.
/// </summary>
public class CartItemBuilder
{
    private int _id = 1;
    private int _cartId = 1;
    private int _productId = 1;
    private int _vendorId = 1;
    private string _productName = "Test Product";
    private int _quantity = 1;
    private decimal _price = 29.99m;

    /// <summary>
    /// Creates a CartItemBuilder with default values.
    /// </summary>
    public static CartItemBuilder Default() => new();

    public CartItemBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CartItemBuilder WithCartId(int cartId)
    {
        _cartId = cartId;
        return this;
    }

    public CartItemBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public CartItemBuilder WithVendorId(int vendorId)
    {
        _vendorId = vendorId;
        return this;
    }

    public CartItemBuilder WithProductName(string productName)
    {
        _productName = productName;
        return this;
    }

    public CartItemBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public CartItemBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    /// <summary>
    /// Builds and returns the CartItem entity.
    /// </summary>
    public CartItem Build()
    {
        return new CartItem
        {
            Id = _id,
            CartId = _cartId,
            ProductId = _productId,
            VendorId = _vendorId,
            ProductName = _productName,
            Quantity = _quantity,
            Price = _price
        };
    }
}
