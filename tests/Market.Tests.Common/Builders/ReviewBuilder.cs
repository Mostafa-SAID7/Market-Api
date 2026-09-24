namespace Market.Tests.Common.Builders;

/// <summary>
/// Fluent builder for creating Review entities for testing.
/// </summary>
public class ReviewBuilder
{
    private int _id = 1;
    private int _productId = 1;
    private int _customerId = 1;
    private int _vendorId = 1;
    private int _ratingValue = 5;
    private string _title = "Great product!";
    private string _comment = "This is a great product. I highly recommend it.";
    private List<ReviewImage> _images = [];
    private bool _isDeleted = false;
    private DateTime _createdAt = DateTime.UtcNow;

    /// <summary>
    /// Creates a ReviewBuilder with default values.
    /// </summary>
    public static ReviewBuilder Default() => new();

    public ReviewBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ReviewBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public ReviewBuilder WithCustomerId(int customerId)
    {
        _customerId = customerId;
        return this;
    }

    public ReviewBuilder WithVendorId(int vendorId)
    {
        _vendorId = vendorId;
        return this;
    }

    public ReviewBuilder WithRating(int ratingValue)
    {
        _ratingValue = ratingValue;
        return this;
    }

    public ReviewBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ReviewBuilder WithComment(string comment)
    {
        _comment = comment;
        return this;
    }

    public ReviewBuilder WithImages(IEnumerable<ReviewImage> images)
    {
        _images = images.ToList();
        return this;
    }

    public ReviewBuilder AddImage(string imageUrl, string? caption = null)
    {
        _images.Add(new ReviewImage
        {
            ImageUrl = imageUrl
        });
        return this;
    }

    public ReviewBuilder AsDeleted(bool isDeleted = true)
    {
        _isDeleted = isDeleted;
        return this;
    }

    public ReviewBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    /// <summary>
    /// Builds and returns the Review entity.
    /// </summary>
    public Review Build()
    {
        return new Review
        {
            Id = _id,
            ProductId = _productId,
            CustomerId = _customerId,
            VendorId = _vendorId,
            RatingValue = _ratingValue,
            Title = _title,
            Comment = _comment,
            Images = _images,
            IsDeleted = _isDeleted,
            CreatedAt = _createdAt
        };
    }
}
