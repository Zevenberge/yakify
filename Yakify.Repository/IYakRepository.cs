using Yakify.Domain;

namespace Yakify.Repository;

public interface IYakRepository: IRepository
{
    Task AddRange(Yak[] yaks, CancellationToken cancellationToken);
    Task<Yak[]> GetAll(CancellationToken cancellationToken);
    Task DeleteAll(CancellationToken cancellationToken);
}
