using Market.Domain.Entities;
using Market.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for Order entity
    /// </summary>
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary key
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.SubTotal)
                .HasPrecision(18, 2);

            builder.Property(o => o.ShippingCost)
                .HasPrecision(18, 2);

            builder.Property(o => o.Tax)
                .HasPrecision(18, 2);

            builder.Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            builder.Property(o => o.OrderStatus)
                .HasConversion<int>();

            builder.Property(o => o.PaymentStatus)
                .HasConversion<int>();

            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.TrackingNumber)
                .HasMaxLength(100);

            builder.Property(o => o.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(o => o.CustomerId);

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(o => o.OrderStatus);
            builder.HasIndex(o => o.PaymentStatus);
            builder.HasIndex(o => o.CreatedAt);

            // Relationships
            // Order -> Customer (User) configured in UserConfiguration

            // One order can have many items
            builder.HasMany<OrderItem>()
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table
            builder.ToTable("Orders");
        }
    }
}

