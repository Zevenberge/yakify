using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace Yakify.Repository.Tests;

public abstract class RepositoryTests: IDisposable
{
    protected RepositoryTests(ITestOutputHelper testOutput)
    {
        var services = new ServiceCollection();
        services.AddLogging(config => config.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.TestOutput(testOutput)
                .CreateLogger()
        ));
        _keepAliveConnection = new SqliteConnection($"DataSource={Guid.NewGuid()};mode=memory;cache=shared");
        _keepAliveConnection.Open();
        services.AddRepositories(opts => opts.UseSqlite(_keepAliveConnection));
        _serviceProvider = services.BuildServiceProvider();
        _serviceProvider.GetRequiredService<YakifyDbContext>().Database.EnsureCreated();
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly SqliteConnection _keepAliveConnection;
    private bool disposedValue;

    protected async Task RunInScope<TRepository>(Func<TRepository, Task> func)
        where TRepository: class
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        await func(scope.ServiceProvider.GetRequiredService<TRepository>());
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _keepAliveConnection.Close();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}