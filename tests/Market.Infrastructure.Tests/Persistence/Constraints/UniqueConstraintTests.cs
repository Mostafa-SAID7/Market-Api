using Market.Tests.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Tests.Persistence.Constraints;

/// <summary>
/// Relational-provider regression tests for unique constraints. SQL Server-specific
/// filtered-index behavior remains covered by the migration and must be verified in CI
/// against SQL Server when that service is available.
/// </summary>
public class UniqueConstraintTests : IntegrationTestBase
{
    [Fact]
    public async Task SaveChanges_WithTwoActiveVendorsForOneUser_ThrowsDbUpdateException()
    {
        var user = new User
        {
            Email = "vendor@example.test", PasswordHash = "hash", FirstName = "Vendor", LastName = "Owner"
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var userId = user.Id;

        DbContext.Vendors.Add(new Vendor
        {
            UserId = userId, StoreName = "First Store", StoreDescription = "The first test store for this user."
        });
        await DbContext.SaveChangesAsync();

        DbContext.Vendors.Add(new Vendor
        {
            UserId = userId, StoreName = "Second Store", StoreDescription = "The second test store for this user."
        });

        await Assert.ThrowsAsync<DbUpdateException>(() => DbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task SaveChanges_WithDuplicateActiveCustomerProductReview_ThrowsDbUpdateException()
    {
        var customer = new User
        {
            Email = "customer@example.test", PasswordHash = "hash", FirstName = "Customer", LastName = "One"
        };
        var vendorUser = new User
        {
            Email = "review-vendor@example.test", PasswordHash = "hash", FirstName = "Vendor", LastName = "One"
        };
        var category = Category.Create("Testing", "Products used for persistence testing.");
        DbContext.AddRange(customer, vendorUser, category);
        await DbContext.SaveChangesAsync();

        var vendor = new Vendor
        {
            UserId = vendorUser.Id, StoreName = "Testing Store", StoreDescription = "A store used only for persistence tests."
        };
        DbContext.Vendors.Add(vendor);
        await DbContext.SaveChangesAsync();

        var product = new Product
        {
            VendorId = vendor.Id, CategoryId = category.Id, Name = "Test Product",
            Description = "A product used only for relational constraint testing.", Price = 10m, Quantity = 1
        };
        DbContext.Products.Add(product);
        await DbContext.SaveChangesAsync();

        DbContext.Reviews.Add(new Review
        {
            ProductId = product.Id, VendorId = vendor.Id, CustomerId = customer.Id, RatingValue = 5,
            Title = "First review", Comment = "A sufficiently long comment for the first review."
        });
        await DbContext.SaveChangesAsync();

        DbContext.Reviews.Add(new Review
        {
            ProductId = product.Id, VendorId = vendor.Id, CustomerId = customer.Id, RatingValue = 4,
            Title = "Second review", Comment = "A sufficiently long comment for the second review."
        });

        await Assert.ThrowsAsync<DbUpdateException>(() => DbContext.SaveChangesAsync());
    }
}
