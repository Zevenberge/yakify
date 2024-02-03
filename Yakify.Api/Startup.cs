using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Yakify.Api.Services;
using Yakify.Repository;

namespace Yakify.Api;

public static class Startup
{
    public static WebApplication Configure(this WebApplicationBuilder builder)
    {
        builder.AddLogging();
        builder.Services.AddApplicationServices(options =>
            options.UseSqlServer(builder.Configuration["ConnectionString"], 
                o => o.MigrationsAssembly(typeof(YakifyDbContext).Assembly.FullName)
            ));
        var app = builder.Build();
        app.ConfigureRequestPipeline();
        return app;
    }

    private static void AddLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    public static void AddApplicationServices(this IServiceCollection services, Action<DbContextOptionsBuilder> configure)
    {
        services.AddDomainServices();
        services.AddRepositories(configure);
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
            });
        services.AddSwagger();
    }

    private static void ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseSwaggerInDevelopment();
        app.UseHttpsRedirection();
        app.UseTransaction();
        app.MapControllers();
    }
}
