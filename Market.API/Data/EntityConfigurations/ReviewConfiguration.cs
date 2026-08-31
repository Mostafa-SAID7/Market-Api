using Market.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.API.Data.EntityConfigurations
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

            // Relationships
            // Review -> Product (configured in ProductConfiguration)
            // Review -> Vendor (configured in VendorConfiguration)
            // Review -> Customer (User) (configured in UserConfiguration)

            // One review can have many images
            builder.HasMany<ReviewImage>()
                .WithOne(ri => ri.Review)
                .HasForeignKey(ri => ri.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table
            builder.ToTable("Reviews");
        }
    }
}
