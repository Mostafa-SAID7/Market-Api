using Market.API.Models.Entities;
using Market.API.Validators;

namespace Market.API.Configurations
{
    /// <summary>
    /// Configures validation services for the application
    /// </summary>
    public static class ValidatorConfiguration
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            // Register all validators
            services.AddScoped<IValidator<Product>, ProductValidator>();
            services.AddScoped<IValidator<Category>, CategoryValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<Vendor>, VendorValidator>();
            services.AddScoped<IValidator<Order>, OrderValidator>();
            services.AddScoped<IValidator<Cart>, CartValidator>();
            services.AddScoped<IValidator<Review>, ReviewValidator>();

            return services;
        }
    }
}
