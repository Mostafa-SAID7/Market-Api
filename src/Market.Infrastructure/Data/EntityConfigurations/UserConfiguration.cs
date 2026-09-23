using Market.Domain.Entities;
using Market.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for User entity
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.Role)
                .HasConversion<int>();

            // Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // User.VendorId is a denormalized cache/reference field, NOT a FK column.
            // Explicitly map it as a plain scalar to prevent EF convention from treating
            // it as a FK to the Vendors table and creating a duplicate User→Vendor relationship.
            builder.Property(u => u.VendorId);
            builder.Ignore(u => u.Vendor);  // Vendor navigation managed by VendorConfiguration

            // NOTE: Order→User is configured in OrderConfiguration (dependent-side).
            // NOTE: Review→User is configured in ReviewConfiguration (dependent-side).
            // NOTE: Cart→User is configured in CartConfiguration (dependent-side).
            // NOTE: Vendor→User is configured in VendorConfiguration (dependent-side).
            // User.Vendor navigation is Ignored above; User.VendorId is a denormalized field.

            // Table
            builder.ToTable("Users");
        }
    }
}

