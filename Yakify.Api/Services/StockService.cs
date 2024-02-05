using Yakify.Api.Models;
using Yakify.Repository;

namespace Yakify.Api.Services;

public class StockService(HerdService herdService, IOrderRepository orderRepository)
{
    public async Task<StockDto> GetStock(int day, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetUpToDay(day, cancellationToken);
        var produce = await herdService.GetTotalProduce(day, cancellationToken);
        return new StockDto(
            produce.Milk - orders.Sum(o => o.Milk ?? 0),
            produce.Skins - orders.Sum(o => o.Skins ?? 0)
        );
    }
}