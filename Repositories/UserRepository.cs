using Domain.Entities;
using Persistence;

namespace Repositories;

public class UserRepository(Db db)
{
    public async Task Save(User user)
    {
        if (await Get(user.Id) != null)
        {
            await db.ExecuteAsync("UPDATE [User].[Users] SET FullName = @FullName, Email = @Email WHERE Id = @Id",
                new { user.Id, user.FullName, user.Email });
        }
        else
        {
            await db.ExecuteAsync("INSERT INTO [User].[Users] (Id, FullName, Email) VALUES (@Id, @FullName, @Email)",
                new { user.Id, user.FullName, user.Email });
        }
    }

    public async Task Remove(Guid id)
    {
        await db.ExecuteAsync("DELETE FROM [User].[Users] WHERE Id = @Id", new { Id = id });
    }

    public async Task<User?> Get(Guid id)
    {
        return (await db.QueryAsync<User>("SELECT Id, FullName, Email FROM [User].[Users] WHERE Id = @Id",
            new { Id = id })).FirstOrDefault();
    }

    public async Task<IList<User>> GetAll()
    {
        return (await db.QueryAsync<User>("SELECT Id, FullName, Email FROM [User].[Users]")).ToList();
    }
}