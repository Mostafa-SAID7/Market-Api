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

            // Relationships configured in ReviewConfiguration

            // Table
            builder.ToTable("ReviewImages");
        }
    }
}

