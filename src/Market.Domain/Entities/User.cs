using Market.Domain.Common;
using Market.Domain.Enums;

namespace Market.Domain.Entities
{
    /// <summary>
    /// User entity for the e-commerce platform
    /// </summary>
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
        public bool EmailConfirmed { get; set; } = false;

        public int? VendorId { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public Vendor? Vendor { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}


