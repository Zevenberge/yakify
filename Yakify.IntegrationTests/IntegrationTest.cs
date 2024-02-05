using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Yakify.Api;
using Yakify.Repository;

namespace Yakify.IntegrationTests;

[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public abstract class IntegrationTest : IClassFixture<YakifyWebApplicationFactory<Program>>, IDisposable
{
    protected IntegrationTest(YakifyWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YakifyDbContext>();
        dbContext.Database.Migrate();
    }

    public WebApplicationFactory<Program> Factory { get; }

    protected async Task RunWithScopedService<TService>(Func<TService, Task> func)
        where TService : notnull
    {
        using var scope = Factory.Services.CreateScope();
        await func(scope.ServiceProvider.GetRequiredService<TService>());
    }

    protected async Task CreateYak(string name, double age, string sex)
    {
        var client = Factory.CreateClient();
        var response = await client.PostAsJsonAsync("/yak-shop/load", new
        {
            Herd = new[] {
                new {
                    Name = name,
                    Age = age,
                    Sex = sex
                }
            }
        });
        response.EnsureSuccessStatusCode();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                using var scope = Factory.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<YakifyDbContext>();
                dbContext.Database.EnsureDeleted();
            }

            disposedValue = true;
        }
    }
    private bool disposedValue;
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
