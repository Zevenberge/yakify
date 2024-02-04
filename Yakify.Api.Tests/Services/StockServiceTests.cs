using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.Api.Services;
using Yakify.Domain;

namespace Yakify.Api.Tests.Services;

public class StockServiceTests(ITestOutputHelper testOutput) : ServiceTests(testOutput)
{
    [Fact]
    public async Task Current_stock_is_total_produce_if_no_orders_are_placed()
    {
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<HerdService>();
            await service.LoadNewHerd(Herd(
                ("Yak-1", 4, Sex.Female),
                ("Yak-2", 8, Sex.Female),
                ("Yak-3", 9.5, Sex.Female)
            ), CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<StockService>();
            var produce = await service.GetStock(15, CancellationToken.None);
            produce.Milk.Should().BeApproximately(1357.2, 1E-10);
            produce.Skins.Should().Be(4);
        });
    }
}