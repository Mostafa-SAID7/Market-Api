using Market.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.API.Data
{
    /// <summary>
    /// EF Core DbContext for SQL Server - manages all entity sets and configurations
    /// </summary>
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options)
            : base(options)
        {
        }

        #region DbSets

        /// <summary>
        /// Users collection
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Vendors collection
        /// </summary>
        public DbSet<Vendor> Vendors => Set<Vendor>();

        /// <summary>
        /// Products collection
        /// </summary>
        public DbSet<Product> Products => Set<Product>();

        /// <summary>
        /// Categories collection
        /// </summary>
        public DbSet<Category> Categories => Set<Category>();

        /// <summary>
        /// Product tags collection
        /// </summary>
        public DbSet<ProductTag> ProductTags => Set<ProductTag>();

        /// <summary>
        /// Orders collection
        /// </summary>
        public DbSet<Order> Orders => Set<Order>();

        /// <summary>
        /// Order items collection
        /// </summary>
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        /// <summary>
        /// Shopping carts collection
        /// </summary>
        public DbSet<Cart> Carts => Set<Cart>();

        /// <summary>
        /// Cart items collection
        /// </summary>
        public DbSet<CartItem> CartItems => Set<CartItem>();

        /// <summary>
        /// Reviews collection
        /// </summary>
        public DbSet<Review> Reviews => Set<Review>();

        /// <summary>
        /// Review images collection
        /// </summary>
        public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarketDbContext).Assembly);
        }
    }
}
