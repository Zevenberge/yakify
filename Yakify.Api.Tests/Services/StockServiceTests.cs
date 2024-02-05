using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.Api.Models;
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

    [Fact]
    public async Task Current_stock_is_produce_minus_orders()
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
            var service = svp.GetRequiredService<OrderService>();
            await service.PlaceOrder(15, new PlaceOrderDto
            {
                Customer = "Garry",
                Order = new OrderDto { Milk = 1300, Skins = 3 },
            }, CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<StockService>();
            var produce = await service.GetStock(15, CancellationToken.None);
            produce.Milk.Should().BeApproximately(57.2, 1E-10);
            produce.Skins.Should().Be(1);
        });
    }

    [Fact]
    public async Task Current_stock_does_not_take_into_account_future_orders()
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
            var service = svp.GetRequiredService<OrderService>();
            await service.PlaceOrder(15, new PlaceOrderDto
            {
                Customer = "Garry",
                Order = new OrderDto { Milk = 1300, Skins = 3 },
            }, CancellationToken.None);
        });
        await RunInTransaction(async svp =>
        {
            var service = svp.GetRequiredService<StockService>();
            var produce = await service.GetStock(10, CancellationToken.None);
            produce.Milk.Should().Be(935.55);
            produce.Skins.Should().Be(3);
        });
    }
}
