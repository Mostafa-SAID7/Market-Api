using System.Diagnostics;

namespace Market.API.Middleware
{
    /// <summary>
    /// Logs HTTP requests and responses for debugging and monitoring.
    /// Skips buffering for Swagger, static files, and non-API routes.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        // Paths that should NEVER be buffered — return them directly
        private static readonly string[] _bypassPrefixes = ["/swagger", "/css", "/js", "/images", "/favicon"];

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Fast-path: skip buffering entirely for Swagger UI assets and static files
            if (ShouldBypass(path))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "HTTP Request: {Method} {Path} - Query: {Query}",
                context.Request.Method,
                path,
                context.Request.QueryString
            );

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "HTTP Response: {Method} {Path} - Status: {StatusCode} - Duration: {ElapsedMilliseconds}ms",
                context.Request.Method,
                path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }

        private static bool ShouldBypass(string path)
        {
            foreach (var prefix in _bypassPrefixes)
                if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
    }
}
