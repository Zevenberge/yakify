using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Yakify.Repository;

public class YakifyDbContextFactory : IDesignTimeDbContextFactory<YakifyDbContext>
{
    public YakifyDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder();
        builder.UseSqlServer("my-connection-string");
        return new YakifyDbContext(builder.Options);
    }
}