using Yakify.Domain;

namespace Yakify.Repository;

public interface IOrderRepository
{
    Task Add(Order order, CancellationToken cancellationToken);
    Task<Order[]> GetAll(CancellationToken cancellationToken);
    Task DeleteAll(CancellationToken cancellationToken);
}