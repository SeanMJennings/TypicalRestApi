using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DatabaseContext<T> : DbContext where T : Aggregate
{
    public DbSet<T> Entities { get; private set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseInMemoryDatabase(databaseName: "crud-api");
}