using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Yakify.Api.Models;
using Yakify.Api.Services;
using Yakify.Domain;
using Yakify.Repository;

namespace Yakify.Api.Tests.Services;

public class OrderServiceTests(ITestOutputHelper testOutput) : ServiceTests(testOutput)
{
    [Fact]
    public async Task Order_within_stock_saves_the_whole_order()
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
            var response = await service.PlaceOrder(15, new PlaceOrderDto
            {
                Customer = "Garry",
                Order = new OrderDto { Milk = 1300, Skins = 3 },
            }, CancellationToken.None);
            response.Should().NotBeNull();
            response!.Milk.Should().Be(1300);
            response.Skins.Should().Be(3);
        });
        await RunInTransaction(async svp =>
        {
            var repository = svp.GetRequiredService<IOrderRepository>();
            var orders = await repository.GetAll(CancellationToken.None);
            orders.Should().HaveCount(1);
            orders[0].Customer.Should().Be("Garry");
            orders[0].Milk.Should().Be(1300);
            orders[0].Skins.Should().Be(3);
        });
    }

    [Fact]
    public async Task Order_partially_within_stock_saves_the_milk()
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
            var response = await service.PlaceOrder(15, new PlaceOrderDto
            {
                Customer = "Garry",
                Order = new OrderDto { Milk = 1300, Skins = 1000 },
            }, CancellationToken.None);
            response.Should().NotBeNull();
            response!.Milk.Should().Be(1300);
            response.Skins.Should().BeNull();
        });
        await RunInTransaction(async svp =>
        {
            var repository = svp.GetRequiredService<IOrderRepository>();
            var orders = await repository.GetAll(CancellationToken.None);
            orders.Should().HaveCount(1);
            orders[0].Customer.Should().Be("Garry");
            orders[0].Milk.Should().Be(1300);
            orders[0].Skins.Should().BeNull();
        });
    }

    [Fact]
    public async Task Order_partially_within_stock_saves_the_skins()
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
            var response = await service.PlaceOrder(15, new PlaceOrderDto
            {
                Customer = "Garry",
                Order = new OrderDto { Milk = 13_000, Skins = 3 },
            }, CancellationToken.None);
            response.Should().NotBeNull();
            response!.Milk.Should().BeNull();
            response.Skins.Should().Be(3);
        });
        await RunInTransaction(async svp =>
        {
            var repository = svp.GetRequiredService<IOrderRepository>();
            var orders = await repository.GetAll(CancellationToken.None);
            orders.Should().HaveCount(1);
            orders[0].Customer.Should().Be("Garry");
            orders[0].Milk.Should().BeNull();
            orders[0].Skins.Should().Be(3);
        });
    }

    [Fact]
    public async Task Order_not_in_stock_does_not_place_order()
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
            var response = await service.PlaceOrder(15, new PlaceOrderDto
            {
                Customer = "Garry",
                Order = new OrderDto { Milk = 13_000, Skins = 300 },
            }, CancellationToken.None);
            response.Should().BeNull();
        });
        await RunInTransaction(async svp =>
        {
            var repository = svp.GetRequiredService<IOrderRepository>();
            var orders = await repository.GetAll(CancellationToken.None);
            orders.Should().HaveCount(0);
        });
    }
}