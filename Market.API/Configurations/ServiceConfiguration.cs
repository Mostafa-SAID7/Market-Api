using Market.API.Data.Interfaces;
using Market.API.Data.Repositories;
using Market.API.Data.UnitOfWork;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// Configures all dependency injection services for the application
    /// Pure CQRS pattern: Handlers call repositories directly, no duplicate service layer
    /// </summary>
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Unit of Work (thin wrapper for transactional support)
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Register generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register domain-specific repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            // Add controllers
            services.AddControllers();

            return services;
        }
    }
}
