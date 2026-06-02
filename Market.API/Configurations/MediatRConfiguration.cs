using MediatR;

namespace Market.API.Configurations
{
    /// <summary>
    /// Configures MediatR services for the application
    /// </summary>
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            // Register MediatR with all handlers in the application
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            return services;
        }
    }
}
