using Microsoft.EntityFrameworkCore;
using Yakify.Domain;

namespace Yakify.Repository;

public class YakifyDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Order { get; set; }
    public DbSet<Yak> Yak { get; set; }
}
