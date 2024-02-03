using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;
using Yakify.TestBase;

namespace Yakify.Repository.Tests;

public abstract class RepositoryTests: SqliteTests
{
    protected RepositoryTests(ITestOutputHelper testOutput)
    {
        var services = new ServiceCollection();
        services.AddLogging(config => config.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.TestOutput(testOutput)
                .CreateLogger()
        ));
        
        services.AddRepositories(opts => opts.UseSqlite(Connection));
        _serviceProvider = services.BuildServiceProvider();
        _serviceProvider.GetRequiredService<YakifyDbContext>().Database.EnsureCreated();
    }

    private readonly IServiceProvider _serviceProvider;

    protected async Task RunInTransaction<TRepository>(Func<TRepository, Task> func)
        where TRepository: class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await func(scope.ServiceProvider.GetRequiredService<TRepository>());
        await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(CancellationToken.None);
    }
}