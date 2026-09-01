namespace Market.API.Middleware
{
    /// <summary>
    /// Extension methods to register all custom middleware
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Register all custom middleware in the correct order
        /// </summary>
        public static WebApplication UseCustomMiddleware(this WebApplication app)
        {
            // Correlation ID must be first to tag all requests
            app.UseMiddleware<CorrelationIdMiddleware>();

            // Request logging should be early to capture details
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Validation middleware
            app.UseMiddleware<ValidationMiddleware>();

            // Exception handling must be near the end but before other middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
