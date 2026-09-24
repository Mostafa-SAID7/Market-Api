using Market.Application.Features.Users;
using Market.Domain.Enums;

namespace Market.Application.Tests.Features.Users
{
    /// <summary>
    /// Tests for UserResponse DTO contract.
    /// Verifies nullability and correct field representation.
    /// </summary>
    public class UserResponseTests
    {
        [Fact]
        public void UserResponse_HasCorrectProperties()
        {
            // Arrange & Act
            var response = new UserResponse
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "+1234567890",
                Role = UserRole.Customer.ToString(),
                IsActive = true,
                IsEmailVerified = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.Equal(1, response.Id);
            Assert.Equal("John", response.FirstName);
            Assert.Equal("Doe", response.LastName);
            Assert.Equal("john@example.com", response.Email);
            Assert.Equal("+1234567890", response.PhoneNumber);
            Assert.True(response.IsActive);
            Assert.True(response.IsEmailVerified);
            Assert.True(response.EmailConfirmed);
        }

        [Fact]
        public void UserResponse_AllowsNullPhoneNumber()
        {
            // Arrange
            var response = new UserResponse
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                PhoneNumber = null, // Nullable
                Role = UserRole.Customer.ToString(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.Null(response.PhoneNumber);
        }

        [Fact]
        public void UserResponse_AllowsNullUpdatedAt()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var response = new UserResponse
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Role = UserRole.Customer.ToString(),
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = null // Nullable
            };

            // Assert
            Assert.Null(response.UpdatedAt);
        }

        [Fact]
        public void UserResponse_DoesNotExposePasswordHash()
        {
            // Arrange
            var response = new UserResponse
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Role = UserRole.Customer.ToString(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Assert - Verify DTO does not have PasswordHash property
            var properties = typeof(UserResponse).GetProperties();
            Assert.DoesNotContain(properties, p => p.Name.Contains("Password") || p.Name.Contains("Hash"));
        }
    }
}
