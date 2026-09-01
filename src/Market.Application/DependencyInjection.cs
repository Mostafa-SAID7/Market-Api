using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Application;

/// <summary>
/// Application layer dependency injection registration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR with all handlers in the Application assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Register all validators from Application assembly
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}

