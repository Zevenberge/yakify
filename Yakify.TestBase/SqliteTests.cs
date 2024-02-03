using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Yakify.TestBase;

public abstract class SqliteTests<TDbContext> : IDisposable
    where TDbContext: DbContext
{
    private bool disposedValue;

    protected SqliteTests(ITestOutputHelper testOutput)
    {
        Connection = new SqliteConnection($"DataSource={Guid.NewGuid()};mode=memory;cache=shared");
        Connection.Open();
        var services = new ServiceCollection();
        services.AddSerilogTestLogger(testOutput);
        RegisterServices(services, opts => opts.UseSqlite(Connection));
        ServiceProvider = services.BuildServiceProvider();
        ServiceProvider.GetRequiredService<TDbContext>().Database.EnsureCreated();
    }

    protected abstract void RegisterServices(IServiceCollection services, Action<DbContextOptionsBuilder> configure);

    protected readonly SqliteConnection Connection;
    protected readonly IServiceProvider ServiceProvider;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Connection.Close();
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
