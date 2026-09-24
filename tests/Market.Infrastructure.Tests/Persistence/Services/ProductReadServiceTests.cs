using Market.Application.Abstractions.Services;
using Market.Application.Features.Products;
using Market.Infrastructure.Persistence.Services;
using Market.Tests.Common.Builders;

namespace Market.Infrastructure.Tests.Persistence.Services
{
    /// <summary>
    /// Tests for ProductReadService SQL-side projections.
    /// Verifies that queries execute efficiently at database level with proper filtering and projection.
    /// </summary>
    public class ProductReadServiceTests : IntegrationTestBase
    {
        private IProductReadService _productReadService = null!;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _productReadService = new ProductReadService(DbContext);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsOnlyActiveProducts()
        {
            // Arrange: Create mix of active and deleted products
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var activeProduct = ProductBuilder.Default()
                .WithName("Active Product")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();

            var deletedProduct = ProductBuilder.WithId(2)
                .WithName("Deleted Product")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            deletedProduct.IsDeleted = true;

            DbContext.AddRange(activeProduct, deletedProduct);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            var products = result.ToList();
            Assert.Single(products);
            Assert.Equal("Active Product", products.First().Name);
            Assert.DoesNotContain(products, p => p.Name == "Deleted Product");
        }

        [Fact]
        public async Task GetAllProductsAsync_ProjectsOnlyRequiredColumns()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product = ProductBuilder.Default()
                .WithName("Test Product")
                .WithPrice(99.99m)
                .WithDiscountPrice(79.99m)
                .WithQuantity(5)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .WithAverageRating(4.5m)
                .WithReviewCount(5)
                .Build();
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            var dto = result.First();
            Assert.Equal(product.Id, dto.Id);
            Assert.Equal("Test Product", dto.Name);
            Assert.Equal(99.99m, dto.Price);
            Assert.Equal(79.99m, dto.DiscountPrice);
            Assert.Equal(5, dto.Quantity);
            Assert.Equal(4.5m, dto.AverageRating);
            Assert.Equal(5, dto.ReviewCount);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsProductWhenExists()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product = ProductBuilder.Default()
                .WithName("Find Me")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal("Find Me", result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsNullWhenNotFound()
        {
            // Act
            var result = await _productReadService.GetProductByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsNullWhenProductIsDeleted()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var deletedProduct = ProductBuilder.Default()
                .WithName("Deleted")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            deletedProduct.IsDeleted = true;
            DbContext.Products.Add(deletedProduct);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductByIdAsync(deletedProduct.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductsByCategoryAsync_ReturnsProductsByCategory()
        {
            // Arrange - Create single category with 2 products
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            // Create 2 products in this category
            var product1 = ProductBuilder.Default()
                .WithName("Product 1 in Cat")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            var product2 = ProductBuilder.WithId(2)
                .WithName("Product 2 in Cat")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();

            DbContext.AddRange(product1, product2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductsByCategoryAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            var products = result.ToList();
            Assert.Equal(2, products.Count);
            Assert.All(products, p => Assert.Equal(category.Id, p.CategoryId));
        }

        [Fact]
        public async Task GetProductsByVendorAsync_ReturnsProductsByVendor()
        {
            // Arrange
            var user1 = UserBuilder.Default().Build();
            var user2 = UserBuilder.WithId(2).WithEmail("user2@example.com").Build();
            var vendor1 = VendorBuilder.Default().WithUserId(user1.Id).Build();
            var vendor2 = VendorBuilder.Default().WithId(2).WithUserId(user2.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user1, user2, vendor1, vendor2, category);
            await DbContext.SaveChangesAsync();

            var product1 = ProductBuilder.Default()
                .WithName("Product by Vendor1")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor1.Id)
                .Build();
            var product2 = ProductBuilder.WithId(2)
                .WithName("Product by Vendor2")
                .WithCategoryId(category.Id)
                .WithVendorId(vendor2.Id)
                .Build();

            DbContext.AddRange(product1, product2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductsByVendorAsync(vendor1.Id);

            // Assert
            Assert.NotNull(result);
            var products = result.ToList();
            Assert.Single(products);
            Assert.Equal(vendor1.Id, products.First().VendorId);
        }

        [Fact]
        public async Task SearchProductsByNameAsync_FindsProductsByName()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product1 = ProductBuilder.Default()
                .WithName("iPhone 15 Pro")
                .WithPrice(999m)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            var product2 = ProductBuilder.WithId(2)
                .WithName("Samsung Galaxy")
                .WithPrice(800m)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();

            DbContext.AddRange(product1, product2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.SearchProductsByNameAsync("iPhone");

            // Assert
            Assert.NotNull(result);
            var products = result.ToList();
            Assert.Single(products);
            Assert.Contains("iPhone", products.First().Name);
        }

        [Fact]
        public async Task SearchProductsByNameAsync_ReturnsEmptyWhenNoMatch()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product = ProductBuilder.Default()
                .WithName("Laptop")
                .WithPrice(1000m)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.SearchProductsByNameAsync("NonExistent");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchProductsByNameAsync_ReturnsEmptyWhenSearchTermIsEmpty()
        {
            // Act
            var result = await _productReadService.SearchProductsByNameAsync("");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_ReturnsProductsInRange()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product1 = ProductBuilder.Default().WithName("Cheap").WithPrice(10m).WithCategoryId(category.Id).WithVendorId(vendor.Id).Build();
            var product2 = ProductBuilder.WithId(2).WithName("Mid").WithPrice(50m).WithCategoryId(category.Id).WithVendorId(vendor.Id).Build();
            var product3 = ProductBuilder.WithId(3).WithName("Expensive").WithPrice(200m).WithCategoryId(category.Id).WithVendorId(vendor.Id).Build();

            DbContext.AddRange(product1, product2, product3);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductsByPriceRangeAsync(25m, 150m);

            // Assert
            Assert.NotNull(result);
            var products = result.ToList();
            Assert.Single(products);
            Assert.Equal("Mid", products.First().Name);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsEmptyWhenNoProducts()
        {
            // Act
            var result = await _productReadService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductsByCategoryAsync_ExcludesDeletedProducts()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var activeProduct = ProductBuilder.Default()
                .WithName("Active")
                .WithPrice(10m)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            var deletedProduct = ProductBuilder.WithId(2)
                .WithName("Deleted")
                .WithPrice(20m)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            deletedProduct.IsDeleted = true;

            DbContext.AddRange(activeProduct, deletedProduct);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductsByCategoryAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            var products = result.ToList();
            Assert.Single(products);
            Assert.Equal("Active", products.First().Name);
        }

        [Fact]
        public async Task GetAllProductsAsync_QueryExecutesOnDatabase_NotInMemory()
        {
            // Arrange - Create large number of products to verify database-side filtering
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            // Create 100 products, 99 deleted and 1 active
            var products = new List<Product>();
            for (int i = 1; i <= 100; i++)
            {
                var product = new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Description = "Test",
                    Price = 10m * i,
                    Quantity = i,
                    CategoryId = category.Id,
                    VendorId = vendor.Id,
                    IsDeleted = i > 1  // Only first is active
                };
                products.Add(product);
            }

            DbContext.AddRange(products);
            await DbContext.SaveChangesAsync();

            // Clear DbContext to ensure query doesn't use in-memory entities
            DbContext.ChangeTracker.Clear();

            // Act
            var result = await _productReadService.GetAllProductsAsync();

            // Assert
            var productsList = result.ToList();
            Assert.Single(productsList);
            Assert.Equal("Product 1", productsList.First().Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsDto_NotEntity()
        {
            // Arrange
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product = ProductBuilder.Default()
                .WithName("DTO Test")
                .WithPrice(50m)
                .WithQuantity(3)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _productReadService.GetProductByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            // Verify it's a DTO by checking type name
            Assert.Equal(typeof(Market.Application.Features.Products.ProductResponse).Name, result.GetType().Name);
        }

        [Fact]
        public async Task SearchProductsByNameAsync_IsCaseSensitiveInSQLite()
        {
            // Arrange - SQLite's default string comparison is case-sensitive
            var user = UserBuilder.Default().Build();
            var vendor = VendorBuilder.Default().WithUserId(user.Id).Build();
            var category = CategoryBuilder.Default().Build();
            DbContext.AddRange(user, vendor, category);
            await DbContext.SaveChangesAsync();

            var product = ProductBuilder.Default()
                .WithName("iPhone 15 Pro")
                .WithPrice(999m)
                .WithCategoryId(category.Id)
                .WithVendorId(vendor.Id)
                .Build();
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            // Act - Search with exact case should work
            var resultExact = await _productReadService.SearchProductsByNameAsync("iPhone");

            // Assert
            Assert.NotEmpty(resultExact);
            
            // Act - Search with different case won't work in SQLite (case-sensitive)
            var resultLower = await _productReadService.SearchProductsByNameAsync("iphone");

            // Assert - SQLite Contains is case-sensitive
            // This will be empty unless case-insensitive collation is configured
            Assert.Empty(resultLower);
        }
    }
}

