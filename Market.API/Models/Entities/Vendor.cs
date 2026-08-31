using Market.API.Common;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// Vendor entity for multi-vendor e-commerce platform
    /// </summary>
    public class Vendor : BaseEntity
    {
        public int UserId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string StoreDescription { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty; // URL to logo
        public string LogoUrl => Logo; // Alias for Logo
        public string Banner { get; set; } = string.Empty; // URL to banner
        public string? Website { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }

        public decimal CommissionRate { get; set; } = 0.10m; // Default 10%
        public bool IsApproved { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public double AverageRating { get; set; } = 0.0;
        public int TotalReviews { get; set; } = 0;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

