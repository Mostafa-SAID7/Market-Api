using Market.API.Common;
using Market.API.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Market.API.Models.Entities
{
    /// <summary>
    /// User entity for the e-commerce platform
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        
        public UserRole Role { get; set; }
        
        public string? VendorId { get; set; } // If user is a vendor
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;

        public string FullName => $"{FirstName} {LastName}";
    }
}

