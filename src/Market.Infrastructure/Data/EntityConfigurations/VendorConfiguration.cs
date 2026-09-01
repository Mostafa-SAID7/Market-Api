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

            builder.Property(v => v.AverageRating)
                .HasPrecision(3, 2);  // 0.00 to 9.99

            // Indexes
            builder.HasIndex(v => v.UserId)
                .IsUnique();

            // Relationships - configured in UserConfiguration
            // One vendor can have many products
            builder.HasMany<Product>()
                .WithOne(p => p.Vendor)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // One vendor can have many reviews
            builder.HasMany<Review>()
                .WithOne(r => r.Vendor)
                .HasForeignKey(r => r.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            builder.ToTable("Vendors");
        }
    }
}

