using Market.API.Middleware;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// Configures HTTP request pipeline and middleware
    /// </summary>
    public static class MiddlewareConfiguration
    {
        public static WebApplication UseApplicationMiddleware(this WebApplication app)
        {
            // Apply custom middleware (error handling, logging, validation, correlation ID)
            app.UseCustomMiddleware();

            // Enable static files (wwwroot)
            app.UseStaticFiles();

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
}
