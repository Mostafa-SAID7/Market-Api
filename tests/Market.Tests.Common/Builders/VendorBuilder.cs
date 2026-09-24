namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating Vendor entities for testing.
/// </summary>
public class VendorBuilder
{
    private int _id = 1;
    private int _userId = 1;
    private string _storeName = "Test Store";
    private string _storeDescription = "A test store";
    private decimal _commissionRate = 0.15m;
    private decimal _averageRating = 4.5m;
    private bool _isDeleted = false;
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates a VendorBuilder with default values.
    /// </summary>
    public static VendorBuilder Default() => new();

    /// <summary>
    /// Creates a VendorBuilder for a specific user.
    /// </summary>
    public static VendorBuilder ForUser(int userId) => new() { _userId = userId };

    public VendorBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public VendorBuilder WithUserId(int userId)
    {
        _userId = userId;
        return this;
    }

    public VendorBuilder WithStoreName(string storeName)
    {
        _storeName = storeName;
        return this;
    }

    public VendorBuilder WithStoreDescription(string description)
    {
        _storeDescription = description;
        return this;
    }

    public VendorBuilder WithCommissionRate(decimal rate)
    {
        _commissionRate = rate;
        return this;
    }

    public VendorBuilder WithAverageRating(decimal rating)
    {
        _averageRating = rating;
        return this;
    }

    public VendorBuilder AsDeleted(bool isDeleted = true)
    {
        _isDeleted = isDeleted;
        return this;
    }

    public VendorBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the Vendor entity.
    /// </summary>
    public Vendor Build()
    {
        return new Vendor
        {
            Id = _id,
            UserId = _userId,
            StoreName = _storeName,
            StoreDescription = _storeDescription,
            CommissionRate = _commissionRate,
            AverageRating = _averageRating,
            IsDeleted = _isDeleted,
            CreatedAt = _createdAt
        };
    }
}
