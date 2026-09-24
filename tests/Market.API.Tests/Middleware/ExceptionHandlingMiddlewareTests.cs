using System.Text;
using Market.API.Middleware;
using Market.Tests.Common.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Market.API.Tests.Middleware;

/// <summary>
/// Tests for ExceptionHandlingMiddleware error sanitization and response formatting.
/// </summary>
public class ExceptionHandlingMiddlewareTests : TestBase
{
    [Fact]
    public async Task InvokeAsync_WithInternalException_ReturnsSafeErrorWithoutDetails()
    {
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("Server=private;Password=not-for-clients"),
            NullLogger<ExceptionHandlingMiddleware>.Instance);
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
    public async Task InvokeAsync_WithNotFound_ReturnsSafeContract()
    {
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new KeyNotFoundException("internal resource key"),
            NullLogger<ExceptionHandlingMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Contains("Resource not found.", body);
        Assert.DoesNotContain("internal resource key", body);
    }
}
