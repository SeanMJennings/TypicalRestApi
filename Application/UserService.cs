using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Repositories;

namespace Application;

public interface IAmAUserService
{
    public Task<User?> Get(Guid id);
    public Task<IList<User>> GetAll();
    public Task<Guid> Add(string name, string email);
    public Task Update(Guid id, string name, string email);
    public Task Remove(Guid id);
}

public class UserService(UserRepository repository) : IAmAUserService
{
    public async Task<User?> Get(Guid id)
    {
        var user = await repository.Get(id);
        return user;
    }

    public async Task<IList<User>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<Guid> Add(string name, string email)
    {
        var id = Guid.NewGuid();
        if ((await repository.GetAll()).Any(u => u.Email == email))
        {
            throw new ValidationException("User with same email already exists");
        }
        var user = new User(id, name, email);
        await repository.Save(user);
        return id;
    }

    public async Task Update(Guid id, string name, string email)
    {
        var user = await repository.Get(id);
        if (user is null) return;
        
        user.UpdateName(name);
        user.UpdateEmail(email);
        await repository.Save(user);
    }

    public async Task Remove(Guid id)
    {
        await repository.Remove(id);
    }
}
