using Market.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services using extension methods from Data.Configurations folder
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddMediatRServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddDataServices(builder.Configuration);

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
await app.InitializeDatabaseAsync();

// Configure HTTP pipeline using extension methods
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
