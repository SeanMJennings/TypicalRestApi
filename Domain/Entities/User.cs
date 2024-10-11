using Domain.Primitives;

namespace Domain.Entities;

public class User(Guid id, Name fullName, Email email) : Aggregate(id)
{
    private User() : this(Guid.Empty, new Name(), new Email()) { }
    
    public Name FullName { get; private set; } = fullName;
    public Email Email { get; private set; } = email;
    public void UpdateName(Name name) => FullName = name;
    public void UpdateEmail(Email email) => Email = email;
}