namespace Market.API.Middleware
{
    /// <summary>
    /// Middleware for handling entity validation errors
    /// </summary>
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new
            {
                success = false,
                message = "Validation failed",
                errors = exception.ValidationResult?.ToString() ?? "Unknown validation error"
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    /// Custom exception for validation errors
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; set; }

        public ValidationException(ValidationResult validationResult)
            : base("Validation failed")
        {
            ValidationResult = validationResult;
        }
    }
}
