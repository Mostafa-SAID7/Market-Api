using Market.API.Data.Interfaces;
using Market.API.Data.Repositories;
using Market.API.Data.UnitOfWork;
using Market.API.Services;
using Market.API.Services.Interfaces;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// Configures all dependency injection services for the application
    /// </summary>
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind MongoDB settings from appsettings.json
            services.Configure<MongoDbSettings>(
                configuration.GetSection(nameof(MongoDbSettings)));

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Register generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register product-specific repository
            services.AddScoped<IProductRepository, ProductRepository>();

            // Register category-specific repository
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Register user-specific repository
            services.AddScoped<IUserRepository, UserRepository>();

            // Register vendor-specific repository
            services.AddScoped<IVendorRepository, VendorRepository>();

            // Register order-specific repository
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Register cart-specific repository
            services.AddScoped<ICartRepository, CartRepository>();

            // Register review-specific repository
            services.AddScoped<IReviewRepository, ReviewRepository>();

            // Register application services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IReviewService, ReviewService>();

            // Add controllers
            services.AddControllers();

            return services;
        }
    }
}
