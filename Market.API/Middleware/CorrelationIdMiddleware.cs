namespace Market.API.Middleware
{
    /// <summary>
    /// Adds correlation ID to each request for request tracing and debugging
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeaderKey = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get or generate correlation ID
            var correlationId = context.Request.Headers.ContainsKey(CorrelationIdHeaderKey)
                ? context.Request.Headers[CorrelationIdHeaderKey].ToString()
                : Guid.NewGuid().ToString();

            // Add to response headers
            context.Response.Headers[CorrelationIdHeaderKey] = correlationId;

            // Store in context items for use in other middleware/handlers
            context.Items[CorrelationIdHeaderKey] = correlationId;

            await _next(context);
        }
    }
}
