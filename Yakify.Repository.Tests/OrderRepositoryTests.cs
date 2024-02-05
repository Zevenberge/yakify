using Xunit.Abstractions;
using Yakify.Domain;

namespace Yakify.Repository.Tests;

public class OrderRepositoryTests(ITestOutputHelper testOutput) : RepositoryTests(testOutput)
{
    [Fact]
    public async Task Orders_can_be_added_and_retreived()
    {
        var order = new Order("Mary", 23.4, 3, 4);
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            await repository.Add(order, CancellationToken.None);
        });
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            var orders = await repository.GetAll(CancellationToken.None);
            orders.Should().HaveCount(1);
            orders[0].Customer.Should().Be("Mary");
            orders[0].Milk.Should().Be(23.4);
            orders[0].Skins.Should().Be(3);
        });
    }

    [Fact]
    public async Task Orders_can_be_deleted()
    {
        var order = new Order("Harry", 23.4, 3, 4);
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            await repository.Add(order, CancellationToken.None);
        });
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            await repository.DeleteAll(CancellationToken.None);
        });
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            var orders = await repository.GetAll(CancellationToken.None);
            orders.Should().HaveCount(0);
        });
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Orders_up_to_including_the_day_they_were_ordered_for_can_be_found(int day)
    {
        var order = new Order("Mary", 12, 34, 3);
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            await repository.Add(order, CancellationToken.None);
        });
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            var orders = await repository.GetUpToDay(day, CancellationToken.None);
            orders.Should().HaveCount(1);
        });
    }

    [Fact]
    public async Task Orders_after_the_day_are_not_found()
    {
        var order = new Order("Mary", 12, 34, 3);
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            await repository.Add(order, CancellationToken.None);
        });
        await RunInTransaction<IOrderRepository>(async repository =>
        {
            var orders = await repository.GetUpToDay(2, CancellationToken.None);
            orders.Should().HaveCount(0);
        });
    }
}