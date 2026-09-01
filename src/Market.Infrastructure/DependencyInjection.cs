using Market.Domain.Repositories;
using Market.Infrastructure.Data;
using Market.Infrastructure.Data.Persistence;
using Market.Infrastructure.Data.Repositories;
using Market.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Market.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection registration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext with SQL Server
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("DefaultConnection connection string not found in configuration.");
        }

        services.AddDbContext<MarketDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register data seeder
        services.AddScoped<DataSeeder>();

        return services;
    }

    /// <summary>
    /// Initialize the database with migrations and seed data.
    /// This method should be called from the API layer (Program.cs)
    /// </summary>
    public static async Task InitializeDatabaseAsync(IServiceProvider services, ILogger logger)
    {
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MarketDbContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

            try
            {
                logger.LogInformation("Initializing database...");
                
                // Apply any pending migrations and create database
                await context.Database.MigrateAsync();
                
                // Seed data
                await seeder.SeedAsync();
                
                logger.LogInformation("Database initialization completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error initializing database");
            }
        }
    }
}
