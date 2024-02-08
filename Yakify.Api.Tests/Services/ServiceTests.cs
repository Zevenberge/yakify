using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.Api.Models;
using Yakify.Domain;
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
        await scope.ServiceProvider.GetRequiredService<YakifyDbContext>().SaveChangesAsync(CancellationToken.None);
    }

    protected CreateHerdDto Herd(params (string Name, double Age, Sex Sex)[] herd)
    {
        return new CreateHerdDto(herd.Select(yak => new CreateYakDto
        {
            Name = yak.Name,
            Age = yak.Age,
            Sex = yak.Sex,
        }).ToArray());
    }
}