using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.TestBase;

namespace Yakify.Repository.Tests;

public abstract class RepositoryTests(ITestOutputHelper testOutput) : SqliteTests<YakifyDbContext>(testOutput)
{
    protected override void RegisterServices(IServiceCollection services, Action<DbContextOptionsBuilder> configure)
    {
        services.AddRepositories(configure);
    }

    protected async Task RunInTransaction<TRepository>(Func<TRepository, Task> func)
        where TRepository: class
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        await func(scope.ServiceProvider.GetRequiredService<TRepository>());
        await scope.ServiceProvider.GetRequiredService<YakifyDbContext>().SaveChangesAsync(CancellationToken.None);
    }
}