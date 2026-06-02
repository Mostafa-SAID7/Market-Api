using Market.API.Data.Seeds;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// Configures data access and database services
    /// </summary>
    public static class DataConfiguration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            // Register MongoDB context
            services.AddSingleton<MongoDbContext>();

            // Register data seeder
            services.AddScoped<DataSeeder>();

            return services;
        }

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Initializing database...");
                    await context.InitializeAsync();
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
