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
            // Add security headers to all responses (fixes CodeQL: cs/web/missing-x-frame-options)
            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
                await next();
            });

            // Serve static files first so SendFileAsync writes go to the original response stream
            app.UseStaticFiles();

            // Apply custom middleware (error handling, logging, validation, correlation ID)
            // Placing custom middleware after static files avoids replacing the response body
            // for SendFile operations which can cause Content-Length mismatches.
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
}
