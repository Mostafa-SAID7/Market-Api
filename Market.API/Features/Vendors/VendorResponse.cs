namespace Market.API.Features.Vendors
{
    /// <summary>
    /// Vendor response DTO
    /// </summary>
    public class VendorResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public string StoreDescription { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Banner { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public decimal CommissionRate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
