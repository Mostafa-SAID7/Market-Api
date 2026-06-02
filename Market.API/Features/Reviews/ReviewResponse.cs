namespace Market.API.Features.Reviews
{
    /// <summary>
    /// Review response DTO
    /// </summary>
    public class ReviewResponse
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public int RatingValue { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public int HelpfulCount { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
