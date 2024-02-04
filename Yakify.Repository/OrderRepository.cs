using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yakify.Domain;

namespace Yakify.Repository;

public class OrderRepository(YakifyDbContext context, ILogger<OrderRepository> logger) : IOrderRepository
{
    public async Task Add(Order order, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding order");
        await context.Order.AddAsync(order, cancellationToken);
    }

    public Task DeleteAll(CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting all orders");
        return context.Order.ExecuteDeleteAsync();
    }

    public Task<Order[]> GetAll(CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting all orders");
        return context.Order.ToArrayAsync();
    }
}