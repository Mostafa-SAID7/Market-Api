using Market.API;
using Market.Application;
using Market.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services from layers using extension methods
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddPresentationServices();

var app = builder.Build();

// Register global crash handlers to log unexpected terminations
AppDomain.CurrentDomain.UnhandledException += (s, e) =>
{
    var ex = e.ExceptionObject as Exception;
    var logger = app.Services.GetService<ILogger<Program>>();
    logger?.LogCritical(ex, "Unhandled AppDomain exception");
};

TaskScheduler.UnobservedTaskException += (s, e) =>
{
    var logger = app.Services.GetService<ILogger<Program>>();
    logger?.LogError(e.Exception, "Unobserved task exception");
    e.SetObserved();
};

// Initialize database
var dbLogger = app.Services.GetRequiredService<ILogger<Program>>();
await Market.Infrastructure.DependencyInjection.InitializeDatabaseAsync(app.Services, dbLogger);

// Configure HTTP pipeline
app.UseSwaggerDocumentation();
app.UseApplicationMiddleware();
app.MapApplicationRoutes();

try
{
    app.Run();
}
catch (Exception ex)
{
    var logger = app.Services.GetService<ILogger<Program>>();
    logger?.LogCritical(ex, "Host terminated unexpectedly");
    throw;
}
