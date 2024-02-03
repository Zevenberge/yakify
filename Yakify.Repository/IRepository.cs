namespace Yakify.Repository;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
