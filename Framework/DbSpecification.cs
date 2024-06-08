using Persistence;

namespace Framework;

public class DbSpecification<T> : Specification where T : Aggregate
{
    protected DatabaseContext<T> database_context = null!;

    protected override void before_each()
    {
        base.before_each();
        database_context = new DatabaseContext<T>();
        database_context.Database.EnsureCreated();
    }

    protected override void after_each()
    {
        database_context.Database.EnsureDeleted();
        database_context.Dispose();
        base.after_each();
    }
}