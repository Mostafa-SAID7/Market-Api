namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating OrderItem entities for testing.
/// </summary>
public class OrderItemBuilder
{
    private int _id = 1;
    private int _orderId = 1;
    private int _productId = 1;
    private int _vendorId = 1;
    private string _productName = "Test Product";
    private int _quantity = 1;
    private decimal _price = 29.99m;

    /// <summary>
    /// Creates an OrderItemBuilder with default values.
    /// </summary>
    public static OrderItemBuilder Default() => new();

    public OrderItemBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public OrderItemBuilder WithOrderId(int orderId)
    {
        _orderId = orderId;
        return this;
    }

    public OrderItemBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public OrderItemBuilder WithVendorId(int vendorId)
    {
        _vendorId = vendorId;
        return this;
    }

    public OrderItemBuilder WithProductName(string productName)
    {
        _productName = productName;
        return this;
    }

    public OrderItemBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderItemBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    /// <summary>
    /// Builds and returns the OrderItem entity.
    /// </summary>
    public OrderItem Build()
    {
        return new OrderItem
        {
            Id = _id,
            OrderId = _orderId,
            ProductId = _productId,
            VendorId = _vendorId,
            ProductName = _productName,
            Quantity = _quantity,
            Price = _price
        };
    }
}
