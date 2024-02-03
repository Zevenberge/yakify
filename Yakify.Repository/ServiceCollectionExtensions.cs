using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Yakify.Repository;

public static class ServiceCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection services, Action<DbContextOptionsBuilder> configure)
    {
        services.AddDbContext<YakifyDbContext>(configure);
        services.AddScoped<IYakRepository, YakRepository>();
        services.AddScoped<IUnitOfWork>(svc => svc.GetRequiredService<YakifyDbContext>());
    }
}