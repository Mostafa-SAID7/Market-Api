using Market.API.Entities;
using Market.API.Repository;
using Market.API.Settings;

var builder = WebApplication.CreateBuilder(args);

// Bind mongodb settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Add services to the container.
// Add the Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Add Product Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Market API",
        Version = "v1",
        Description = "A RESTful API for managing products with MongoDB",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Mostafa SAID",
            Url = new Uri("https://github.com/Mostafa-SAID7/Market-Api")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Market API v1");
        options.RoutePrefix = "swagger";
    });
}

// Enable static files (wwwroot)
var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("index.html");
app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Custom 404 handling for non-API routes
app.Use(async (context, next) =>
{
    await next();
    
    // If the response is 404 and it's not an API call
    if (context.Response.StatusCode == 404 && !context.Request.Path.StartsWithSegments("/api"))
    {
        context.Request.Path = "/404.html";
        await next();
    }
});

app.Run();
