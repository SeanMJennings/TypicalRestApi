using Microsoft.EntityFrameworkCore;

namespace Persistence;

[PrimaryKey(nameof(Id))]
public abstract class Aggregate(Guid Id)
{
    public Guid Id { get; } = Id;
}