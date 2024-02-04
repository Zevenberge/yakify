using Yakify.Api.Models;

namespace Yakify.Api.Services;

public class StockService(HerdService herdService)
{
    public async Task<StockDto> GetStock(int day, CancellationToken cancellationToken)
    {
        return await herdService.GetTotalProduce(day, cancellationToken);
    }
}