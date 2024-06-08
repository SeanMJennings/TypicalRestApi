using Domain.Primitives;
using Persistence;

namespace Domain.Entities;

public class User(Guid id, Name name, Email email) : Aggregate(id)
{
    public Name Name { get; private set; } = name;
    public Email Email { get; private set; } = email;
    public void UpdateName(Name name) => Name = name;
    public void UpdateEmail(Email email) => Email = email;
}