using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public interface IAmARepository<T> where T : Aggregate
{
    public Task Save(T entity);
    public Task Remove(Guid id);
    public Task<T?> Get(Guid id);
    public Task<IReadOnlyList<T>> GetAll();
}

public class InMemoryRepository<T> : IAmARepository<T>
    where T : Aggregate
{
    private DatabaseContext<T> DbContext { get; }

    public InMemoryRepository(DatabaseContext<T> DbContext)
    {
        this.DbContext = DbContext;
        this.DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public async Task Save(T entity)
    {
        if (await Get(entity.Id) != null)
        {
            try
            {
                DbContext.Entities.Update(entity);
            }
            catch (InvalidOperationException)
            {
                throw new ValidationException($"{typeof(T).Name} with same ID already exists");
            }
        }
        else
        {
            await DbContext.Entities.AddAsync(entity);
        }

        await DbContext.SaveChangesAsync();
    }

    public async Task Remove(Guid id)
    {
        var entity = await Get(id);
        if (entity == null) return;
        DbContext.Entities.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task<T?> Get(Guid id)
    {
        var user = await DbContext.Entities.FindAsync(id);
        return user;
    }

    public async Task<IReadOnlyList<T>> GetAll()
    {
        var results = new ReadOnlyCollection<T>(await DbContext.Entities.ToListAsync());
        return results;
    }
}