using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for CartItem (product in cart)
    /// </summary>
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            // Primary key
            builder.HasKey(ci => ci.Id);

            // Properties
            builder.Property(ci => ci.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.Price)
                .HasPrecision(18, 2);

            builder.Property(ci => ci.ImageUrl)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(ci => ci.CartId);
            builder.HasIndex(ci => ci.ProductId);
            builder.HasIndex(ci => ci.VendorId);

            // Relationships
            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ci => ci.Vendor)
                .WithMany()
                .HasForeignKey(ci => ci.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            builder.ToTable("CartItems");
        }
    }
}

