using Market.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services using extension methods from Data.Configurations folder
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddMediatRServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddDataServices();

var app = builder.Build();

// Initialize database
await app.InitializeDatabaseAsync();

// Configure HTTP pipeline using extension methods
app.UseSwaggerDocumentation();
app.UseApplicationMiddleware();
app.MapApplicationRoutes();

app.Run();
