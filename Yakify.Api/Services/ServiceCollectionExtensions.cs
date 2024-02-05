namespace Yakify.Api.Services;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<HerdService>();
        services.AddScoped<StockService>();
        services.AddScoped<OrderService>();
    }
}