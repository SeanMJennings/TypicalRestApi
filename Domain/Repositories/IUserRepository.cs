using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task Save(User user);
    Task Remove(Guid id);
    Task<User?> Get(Guid id);
    Task<IList<User>> GetAll();
}
