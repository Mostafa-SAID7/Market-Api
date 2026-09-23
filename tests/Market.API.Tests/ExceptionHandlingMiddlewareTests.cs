using Xunit;
using System.Text;
using Market.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Market.API.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_InternalException_ReturnsSafeErrorWithoutExceptionDetails()
    {
        var middleware = new ExceptionHandlingMiddleware(_ => throw new InvalidOperationException("Server=private;Password=not-for-clients"), NullLogger<ExceptionHandlingMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Contains("An unexpected error occurred.", body);
        Assert.DoesNotContain("Password", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("private", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task InvokeAsync_NotFound_ReturnsSafeContract()
    {
        var middleware = new ExceptionHandlingMiddleware(_ => throw new KeyNotFoundException("internal resource key"), NullLogger<ExceptionHandlingMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Contains("Resource not found.", body);
        Assert.DoesNotContain("internal resource key", body);
    }

    [Fact]
    public async Task InvokeAsync_AddsSecurityHeadersBeforeCallingNextMiddleware()
    {
        var middleware = new SecurityHeadersMiddleware(_ => Task.CompletedTask);
        var context = new DefaultHttpContext();

        await middleware.InvokeAsync(context);

        Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
        Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
        Assert.Equal("strict-origin-when-cross-origin", context.Response.Headers["Referrer-Policy"]);
        Assert.Equal("geolocation=(), microphone=(), camera=()", context.Response.Headers["Permissions-Policy"]);
    }
}
