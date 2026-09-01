using Market.API.Middleware;
using Microsoft.OpenApi.Models;

namespace Market.API;

/// <summary>
/// Presentation layer dependency injection registration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        // Add controllers
        services.AddControllers();

        // Add endpoints API explorer and Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Market API",
                Version = "v1",
                Description = "A RESTful API for e-commerce platform",
                Contact = new OpenApiContact
                {
                    Name = "Mostafa SAID",
                    Url = new Uri("https://github.com/Mostafa-SAID7/Market-Api")
                }
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Market API v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }

    public static WebApplication UseApplicationMiddleware(this WebApplication app)
    {
        // Add security headers to all responses
        app.Use(async (context, next) =>
        {
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
            await next();
        });

        // Serve static files
        app.UseStaticFiles();

        // Apply custom middleware (error handling, logging, validation, correlation ID)
        app.UseCustomMiddleware();

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Enable authorization
        app.UseAuthorization();

        return app;
    }

    public static WebApplication MapApplicationRoutes(this WebApplication app)
    {
        // Redirect root to index.html
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/index.html", permanent: false);
            return Task.CompletedTask;
        });

        // Map all controller routes
        app.MapControllers();

        // Fallback for 404 - must be last
        app.MapFallback(async context =>
        {
            var path = context.Request.Path.Value;

            // Don't handle API or Swagger routes - let them 404 naturally
            if (path?.StartsWith("/api") == true || path?.StartsWith("/swagger") == true)
            {
                context.Response.StatusCode = 404;
                return;
            }

            // Show 404 page for all other non-existent routes
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync("wwwroot/404.html");
        });

        return app;
    }
}
