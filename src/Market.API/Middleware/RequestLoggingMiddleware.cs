using System.Diagnostics;

namespace Market.API.Middleware
{
    /// <summary>
    /// Logs HTTP requests and responses for debugging and monitoring
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log request
            _logger.LogInformation(
                "HTTP Request: {Method} {Path} - Query: {Query}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString
            );

            // Store original response body stream
            var originalBodyStream = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();

                    // Log response
                    _logger.LogInformation(
                        "HTTP Response: {Method} {Path} - Status: {StatusCode} - Duration: {ElapsedMilliseconds}ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds
                    );

                    // Copy response back to original stream
                            // Rewind the memory stream so its contents are copied from the start
                            memoryStream.Position = 0;
                            await memoryStream.CopyToAsync(originalBodyStream);

                            // Restore the original response body stream so downstream nothing is affected
                            context.Response.Body = originalBodyStream;
                }
            }
        }
    }
}
