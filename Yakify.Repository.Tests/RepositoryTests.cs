using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace Yakify.Repository.Tests;

public abstract class RepositoryTests
{
    protected RepositoryTests(ITestOutputHelper testOutput)
    {
        var services = new ServiceCollection();
        services.AddLogging(config => config.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.TestOutput(testOutput)
                .CreateLogger()
        ));
        services.AddRepositories(opts => opts.UseInMemoryDatabase("database"));
        _serviceProvider = services.BuildServiceProvider();
    }

    private readonly IServiceProvider _serviceProvider;

    protected async Task RunInScope<TRepository>(Func<TRepository, Task> func)
        where TRepository: class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await func(scope.ServiceProvider.GetRequiredService<TRepository>());
    }
}