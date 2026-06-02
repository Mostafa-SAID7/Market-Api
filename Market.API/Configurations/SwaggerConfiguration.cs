using Microsoft.OpenApi.Models;

namespace Market.API.Data.Configurations
{
    /// <summary>
    /// Configures Swagger/OpenAPI documentation
    /// </summary>
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Market API",
                    Version = "v1",
                    Description = "A RESTful API for managing products with MongoDB",
                    Contact = new OpenApiContact
                    {
                        Name = "Mostafa SAID",
                        Url = new Uri("https://github.com/Mostafa-SAID7/Market-Api")
                    }
                });
            });

            return services;
        }

        public static WebApplication UseSwaggerDocumentation(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Market API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            return app;
        }
    }
}
