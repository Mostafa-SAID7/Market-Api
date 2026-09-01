using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for OrderItem (order line item)
    /// </summary>
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Primary key
            builder.HasKey(oi => oi.Id);

            // Properties
            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // Indexes
            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.ProductId);
            builder.HasIndex(oi => oi.VendorId);

            // Relationships
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.Vendor)
                .WithMany()
                .HasForeignKey(oi => oi.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            builder.ToTable("OrderItems");
        }
    }
}

