using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for ReviewImage (image in a review)
    /// </summary>
    public class ReviewImageConfiguration : IEntityTypeConfiguration<ReviewImage>
    {
        public void Configure(EntityTypeBuilder<ReviewImage> builder)
        {
            // Primary key
            builder.HasKey(ri => ri.Id);

            // Properties
            builder.Property(ri => ri.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(ri => ri.ReviewId);

            // ReviewImage → Review (ReviewImage is dependent, owns ReviewId FK)
            // Single authoritative definition — prevents ReviewImage.ReviewId1 shadow FK
            // that occurs when ReviewConfiguration also configures HasMany<ReviewImage>.
            builder.HasOne(ri => ri.Review)
                .WithMany(r => r.Images)
                .HasForeignKey(ri => ri.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table
            builder.ToTable("ReviewImages");
        }
    }
}

