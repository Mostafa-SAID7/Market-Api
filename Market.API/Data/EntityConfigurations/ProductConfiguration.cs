using Market.API.Models.Entities;
using Market.API.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.API.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for Product entity
    /// </summary>
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Property(p => p.DiscountPrice)
                .HasPrecision(18, 2);

            builder.Property(p => p.SKU)
                .HasMaxLength(100);

            builder.Property(p => p.Status)
                .HasConversion<int>();

            builder.Property(p => p.AverageRating)
                .HasPrecision(3, 2);  // 0.00 to 9.99

            // Indexes
            builder.HasIndex(p => p.VendorId);
            builder.HasIndex(p => p.CategoryId);

            builder.HasIndex(p => p.SKU)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0 AND [SKU] IS NOT NULL");

            builder.HasIndex(p => p.Status);

            // Relationships
            // Product -> Category (configured in CategoryConfiguration)
            // Product -> Vendor (configured in VendorConfiguration)

            // One product can have many tags
            builder.HasMany<ProductTag>()
                .WithOne(pt => pt.Product)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // One product can have many reviews
            builder.HasMany<Review>()
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // One product can appear in many order items
            builder.HasMany<OrderItem>()
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // One product can appear in many cart items
            builder.HasMany<CartItem>()
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            builder.ToTable("Products");
        }
    }
}
