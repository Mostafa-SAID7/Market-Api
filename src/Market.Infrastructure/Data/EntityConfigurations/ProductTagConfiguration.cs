using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for ProductTag (junction table for Product tags)
    /// </summary>
    public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            // Primary key
            builder.HasKey(pt => pt.Id);

            // Properties
            builder.Property(pt => pt.TagName)
                .IsRequired()
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(pt => new { pt.ProductId, pt.TagName })
                .IsUnique();

            // Relationships configured in ProductConfiguration

            // Table
            builder.ToTable("ProductTags");
        }
    }
}

