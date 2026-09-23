using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for Review entity
    /// </summary>
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Primary key
            builder.HasKey(r => r.Id);

            // Properties
            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.Comment)
                .HasMaxLength(2000);

            builder.Property(r => r.RatingValue)
                .HasConversion<int>();

            // Indexes
            builder.HasIndex(r => r.ProductId);
            builder.HasIndex(r => r.VendorId);
            builder.HasIndex(r => r.CustomerId);
            builder.HasIndex(r => r.RatingValue);
            builder.HasIndex(r => r.CreatedAt);

            // NOTE: ReviewImage→Review relationship is configured in ReviewImageConfiguration
            // (dependent-side). Configuring HasMany<ReviewImage> here from the Review side
            // creates a duplicate shadow FK column (ReviewImage.ReviewId1).
            // NOTE: Review→Product, Review→Vendor, Review→Customer(User) relationships
            // are configured in ReviewConfiguration as the dependent owns those FKs.

            // Review → Product (Review is dependent, owns ProductId FK)
            builder.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review → Vendor (Review is dependent, owns VendorId FK)
            builder.HasOne(r => r.Vendor)
                .WithMany(v => v.Reviews)
                .HasForeignKey(r => r.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review → User/Customer (Review is dependent, owns CustomerId FK)
            builder.HasOne(r => r.Customer)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            builder.ToTable("Reviews");
        }
    }
}

