using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yakify.Domain;

namespace Yakify.Repository;

public class YakRepository(YakifyDbContext context, ILogger<YakRepository> logger) : IYakRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task AddRange(Yak[] yaks, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding {AmountOfYaks}", yaks.Length);
        return context.AddRangeAsync(yaks, cancellationToken);
    }

    public Task<Yak[]> GetAll(CancellationToken cancellationToken)
    {
        logger.LogDebug("Retreiving all yaks");
        return context.Yak.ToArrayAsync(cancellationToken);
    }

    public Task DeleteAll(CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting all yaks");
        return context.Yak.ExecuteDeleteAsync(cancellationToken);
    }
}
