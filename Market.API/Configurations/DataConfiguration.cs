using Market.API.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// Configures data access and database services for EF Core
    /// </summary>
    public static class DataConfiguration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register EF Core DbContext with SQL Server
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection connection string not found in configuration.");
            }

            services.AddDbContext<MarketDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register data seeder
            services.AddScoped<DataSeeder>();

            return services;
        }

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MarketDbContext>();
                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

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
}
