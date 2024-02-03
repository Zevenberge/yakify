using Microsoft.EntityFrameworkCore;
using Yakify.Domain;

namespace Yakify.Repository;

public class YakifyDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<Yak> Yak { get; set; }

    Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }
}