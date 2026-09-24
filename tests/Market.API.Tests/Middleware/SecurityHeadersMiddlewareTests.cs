using Market.API.Middleware;
using Market.Tests.Common.Base;
using Microsoft.AspNetCore.Http;

namespace Market.API.Tests.Middleware;

/// <summary>
/// Tests for SecurityHeadersMiddleware header injection.
/// </summary>
public class SecurityHeadersMiddlewareTests : TestBase
{
    [Fact]
    public async Task InvokeAsync_AddsSecurityHeadersBeforeCallingNext()
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
