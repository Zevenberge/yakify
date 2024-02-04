using Microsoft.EntityFrameworkCore;
using Serilog;
using Yakify.Repository;

namespace Yakify.Api;

public static class MigrationExtensions
{
    public static Task RunMigrations(this WebApplication app)
    {
        if(app.Configuration["RunMigrations"] == "true")
        {
            return RunEfMigrations(app);
        }
        return Task.CompletedTask;
    }

    private static async Task RunEfMigrations(WebApplication app)
    {
        Log.Logger.Information("Running mgirations");
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YakifyDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}