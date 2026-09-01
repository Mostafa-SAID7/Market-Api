using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for Category entity with self-referencing hierarchy
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.DisplayOrder)
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(c => c.Slug)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Self-referencing hierarchy for subcategories
            builder.HasOne(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships
            // One category can have many products
            builder.HasMany<Product>()
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table
            builder.ToTable("Categories");
        }
    }
}

