using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.Repository;
using Yakify.TestBase;

namespace Yakify.Api.Tests.Services;

public abstract class ServiceTests(ITestOutputHelper testOutput) : SqliteTests<YakifyDbContext>(testOutput)
{
    protected override void RegisterServices(IServiceCollection services, Action<DbContextOptionsBuilder> configure)
    {
        services.AddApplicationServices(configure);
    }

    protected async Task RunInTransaction(Func<IServiceProvider, Task> func)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        await func(scope.ServiceProvider);
        await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(CancellationToken.None);
    }
}