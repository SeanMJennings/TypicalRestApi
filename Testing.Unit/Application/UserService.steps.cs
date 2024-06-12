using Application;
using Domain.Entities;
using FluentAssertions;
using Framework;
using Persistence;

namespace Unit.Application;

public partial class UserServiceSpecs : DbSpecification<User>
{
    private Guid id;
    private Guid another_id;
    private User retrieved_entity = null!;
    private IReadOnlyList<User> entities = null!;
    private IAmARepository<User> repository = null!;
    private UserService _userService = null!;

    private const string name = "wibble";
    private const string email = "wobble@gmail.com";
    private const string new_name = "wobble";
    private const string new_email = "wibble@gmail.com";

    protected override void before_each()
    {
        base.before_each();
        id = default;
        another_id = default;
        entities = null!;
        retrieved_entity = null!;
        repository = new InMemoryRepository<User>(database_context);
        _userService = new UserService(repository);
    }
    
    private void user_details(){}     
    
    private void adding_a_user()
    {
        id = _userService.Add(name, email).GetAwaiter().GetResult();
    }    
    
    private void saving_another_user()
    {
        another_id = _userService.Add(new_name, new_email).GetAwaiter().GetResult();
    }
    
    private void updating_a_user()
    {
        _userService.Update(id, new_name, new_email).GetAwaiter().GetResult();
        retrieving_a_user();
    }
    
    private void a_user_exists()
    {
        adding_a_user();
    }

    private void another_user_exists()
    {
        saving_another_user();
    }    
    
    private void saving_another_user_with_same_email()
    {
        _userService.Add(new_name, email).GetAwaiter().GetResult();
    }    
    
    private void retrieving_a_user()
    {
        retrieved_entity = _userService.Get(id).GetAwaiter().GetResult()!;
    }        
    
    private void listing_entities()
    {
        entities = _userService.GetAll().GetAwaiter().GetResult();
    }     
    
    private void removing_an_user()
    {
        _userService.Remove(id).GetAwaiter().GetResult();
    }    
    
    private void the_user_is_correct()
    {
        retrieved_entity.Id.Should().Be(id);
        retrieved_entity.Name.ToString().Should().Be(name);
        retrieved_entity.Email.ToString().Should().Be(email);
    }      
    
    private void the_user_is_updated()
    {
        retrieved_entity.Id.Should().Be(id);
        retrieved_entity.Name.ToString().Should().Be(new_name);
        retrieved_entity.Email.ToString().Should().Be(new_email);
    }    
    
    private void the_user_is_null()
    {
        retrieved_entity.Should().Be(null);
    }    
    
    private void the_list_is_correct()
    {
        entities.Count.Should().Be(2);
        entities[0].Id.Should().Be(id);
        entities[0].Name.ToString().Should().Be(name);
        entities[0].Email.ToString().Should().Be(email);
        entities[1].Id.Should().Be(another_id);
        entities[1].Name.ToString().Should().Be(new_name);
        entities[1].Email.ToString().Should().Be(new_email);
    }
}