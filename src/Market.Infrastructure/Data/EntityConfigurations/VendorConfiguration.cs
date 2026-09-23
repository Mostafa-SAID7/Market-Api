using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for Vendor entity
    /// </summary>
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            // Primary key
            builder.HasKey(v => v.Id);

            // Properties
            builder.Property(v => v.StoreName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(v => v.StoreDescription)
                .HasMaxLength(1000);

            builder.Property(v => v.Logo)
                .HasMaxLength(500);

            builder.Property(v => v.Banner)
                .HasMaxLength(500);

            builder.Property(v => v.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(v => v.Address)
                .HasMaxLength(500);

            builder.Property(v => v.City)
                .HasMaxLength(100);

            builder.Property(v => v.Country)
                .HasMaxLength(100);

            builder.Property(v => v.ZipCode)
                .HasMaxLength(20);

            builder.Property(v => v.CommissionRate)
                .HasPrecision(5, 2);  // Up to 999.99%

            // CHECK constraint for commission rate (0% to 50%)
            // Modern approach using ToTable with lambda

            builder.Property(v => v.AverageRating)
                .HasPrecision(3, 2);  // Ratings are stored exactly to two decimal places.

            // Indexes
            builder.HasIndex(v => v.UserId)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Vendor → User (one-to-one, Vendor is dependent — owns UserId FK)
            builder.HasOne(v => v.User)
                .WithOne()
                .HasForeignKey<Vendor>(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // NOTE: Product→Vendor relationship is configured in ProductConfiguration
            // (dependent-side). Configuring HasMany<Product> here from the Vendor side
            // creates a duplicate shadow FK column (Product.VendorId1).

            // NOTE: Review→Vendor relationship is configured in ReviewConfiguration
            // (dependent-side only). Configuring it here would create VendorId1 shadow FK.

            // Table with CHECK constraint
            builder.ToTable("Vendors", t =>
                t.HasCheckConstraint("CK_Vendors_CommissionRate_Range",
                    "[CommissionRate] >= 0.00 AND [CommissionRate] <= 0.50"));
        }
    }
}

