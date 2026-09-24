namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating Order entities for testing.
/// </summary>
public class OrderBuilder
{
    private int _id = 1;
    private int _customerId = 1;
    private List<OrderItem> _items = [];
    private decimal _subtotal = 100m;
    private decimal _tax = 10m;
    private decimal _shippingCost = 5m;
    private string _shippingAddress = "123 Main St";
    private OrderStatus _status = OrderStatus.Pending;
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates an OrderBuilder with default values.
    /// </summary>
    public static OrderBuilder Default() => new();

    /// <summary>
    /// Creates an OrderBuilder for a specific customer.
    /// </summary>
    public static OrderBuilder ForCustomer(int customerId) => new() { _customerId = customerId };

    public OrderBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public OrderBuilder WithCustomerId(int customerId)
    {
        _customerId = customerId;
        return this;
    }

    public OrderBuilder WithItems(IEnumerable<OrderItem> items)
    {
        _items = items.ToList();
        return this;
    }

    public OrderBuilder AddItem(OrderItem item)
    {
        _items.Add(item);
        return this;
    }

    public OrderBuilder AddItem(int productId, int vendorId, int quantity, decimal price)
    {
        _items.Add(new OrderItem
        {
            ProductId = productId,
            VendorId = vendorId,
            ProductName = "Test Product",
            Quantity = quantity,
            Price = price
        });
        return this;
    }

    public OrderBuilder WithSubtotal(decimal subtotal)
    {
        _subtotal = subtotal;
        return this;
    }

    public OrderBuilder WithTax(decimal tax)
    {
        _tax = tax;
        return this;
    }

    public OrderBuilder WithShippingCost(decimal shippingCost)
    {
        _shippingCost = shippingCost;
        return this;
    }

    public OrderBuilder WithShippingAddress(string address)
    {
        _shippingAddress = address;
        return this;
    }

    public OrderBuilder WithStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    public OrderBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the Order entity.
    /// </summary>
    public Order Build()
    {
        return new Order
        {
            Id = _id,
            CustomerId = _customerId,
            Items = _items,
            SubTotal = _subtotal,
            Tax = _tax,
            ShippingCost = _shippingCost,
            ShippingAddress = _shippingAddress,
            OrderStatus = _status,
            CreatedAt = _createdAt,
            TotalPrice = _subtotal + _tax + _shippingCost
        };
    }
}
