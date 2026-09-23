using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.EntityConfigurations
{
    /// <summary>
    /// EF Core configuration for Cart entity
    /// </summary>
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Primary key
            builder.HasKey(c => c.Id);

            // Indexes
            builder.HasIndex(c => c.UserId)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Cart → User (one-to-one, Cart is dependent — owns UserId FK)
            // WithOne(u => u.Cart) ties to User.Cart navigation; prevents EF convention
            // from discovering a second relationship through User.Cart and creating Cart.UserId1.
            builder.HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One cart can have many items
            builder.HasMany<CartItem>()
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table
            builder.ToTable("Carts");
        }
    }
}

