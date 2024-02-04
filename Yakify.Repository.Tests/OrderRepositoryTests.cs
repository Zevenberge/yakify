using Xunit.Abstractions;
using Yakify.Domain;

namespace Yakify.Repository.Tests;

public class OrderRepositoryTests(ITestOutputHelper testOutput) : RepositoryTests(testOutput)
{
    [Fact]
    public async Task Orders_can_be_added_and_retreived()
    {
        var order = new Order("Mary", 23.4, 3);
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
        var order = new Order("Harry", 23.4, 3);
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
}