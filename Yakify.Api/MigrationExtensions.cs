using Microsoft.EntityFrameworkCore;
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
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YakifyDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}