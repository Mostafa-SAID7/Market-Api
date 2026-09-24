using Market.Application.Features.Products;

namespace Market.Application.Tests.Features.Products
{
    /// <summary>
    /// Tests for ProductResponse DTO contract.
    /// Verifies correct fields, types, and nullability.
    /// </summary>
    public class ProductResponseTests
    {
        [Fact]
        public void ProductResponse_HasCorrectProperties()
        {
            // Arrange & Act
            var response = new ProductResponse
            {
                Id = 1,
                Name = "Test Product",
                Description = "A test product",
                Price = 99.99m,
                DiscountPrice = 79.99m,
                ImageUrl = "https://example.com/image.jpg",
                Quantity = 10,
                Sold = 5,
                CategoryId = 1,
                VendorId = 1,
                AverageRating = 4.5m,
                ReviewCount = 10
            };

            // Assert
            Assert.Equal(1, response.Id);
            Assert.Equal("Test Product", response.Name);
            Assert.Equal("A test product", response.Description);
            Assert.Equal(99.99m, response.Price);
            Assert.Equal(79.99m, response.DiscountPrice);
            Assert.Equal("https://example.com/image.jpg", response.ImageUrl);
            Assert.Equal(10, response.Quantity);
            Assert.Equal(5, response.Sold);
            Assert.Equal(1, response.CategoryId);
            Assert.Equal(1, response.VendorId);
            Assert.Equal(4.5m, response.AverageRating);
            Assert.Equal(10, response.ReviewCount);
        }

        [Fact]
        public void ProductResponse_AllowsNullDiscountPrice()
        {
            // Arrange
            var response = new ProductResponse
            {
                Id = 1,
                Name = "No Discount",
                Description = "Test",
                Price = 50m,
                DiscountPrice = null, // Nullable
                CategoryId = 1,
                VendorId = 1
            };

            // Assert
            Assert.Null(response.DiscountPrice);
        }

        [Fact]
        public void ProductResponse_AllowsNullImageUrl()
        {
            // Arrange
            var response = new ProductResponse
            {
                Id = 1,
                Name = "No Image",
                Description = "Test",
                Price = 50m,
                ImageUrl = null, // Nullable
                CategoryId = 1,
                VendorId = 1
            };

            // Assert
            Assert.Null(response.ImageUrl);
        }

        [Fact]
        public void ProductResponse_RequiresNonNullName()
        {
            // Arrange & Act
            var response = new ProductResponse
            {
                Id = 1,
                Name = "", // Empty but not null
                Description = "Test",
                Price = 50m,
                CategoryId = 1,
                VendorId = 1
            };

            // Assert
            Assert.NotNull(response.Name);
            Assert.Equal("", response.Name);
        }

        [Fact]
        public void ProductResponse_SupportsZeroValues()
        {
            // Arrange
            var response = new ProductResponse
            {
                Id = 1,
                Name = "Zero Stock",
                Description = "Test",
                Price = 0m, // Can be zero
                Quantity = 0,
                Sold = 0,
                CategoryId = 1,
                VendorId = 1,
                AverageRating = 0m,
                ReviewCount = 0
            };

            // Assert
            Assert.Equal(0m, response.Price);
            Assert.Equal(0, response.Quantity);
            Assert.Equal(0, response.Sold);
            Assert.Equal(0m, response.AverageRating);
            Assert.Equal(0, response.ReviewCount);
        }
    }
}
