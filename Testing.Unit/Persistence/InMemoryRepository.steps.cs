using Domain.Entities;
using FluentAssertions;
using Framework;
using Persistence;

namespace Unit.Persistence;

public partial class UserServiceSpecs : DbSpecification<User>
{
    private readonly Guid id = Guid.NewGuid();
    private readonly Guid another_id = Guid.NewGuid();
    private User entity = null!;
    private User another_entity = null!;
    private User retrieved_entity = null!;
    private IReadOnlyList<User> entities = null!;
    private IAmARepository<User> repository = null!;

    private const string name = "wibble";
    private const string email = "wobble@gmail.com";
    private const string new_name = "wobble";
    private const string new_email = "wibble@gmail.com";

    protected override void before_each()
    {
        base.before_each();
        entity = null!;
        entities = null!;
        another_entity = null!;
        retrieved_entity = null!;
        repository = new InMemoryRepository<User>(database_context);
    }
    
    private void an_entity()
    {
        entity = new User(id, name,email);
    }    
    
    private void saving_an_entity()
    {
        repository.Save(entity).GetAwaiter().GetResult();
    }    
    
    private void saving_another_entity()
    {
        repository.Save(another_entity).GetAwaiter().GetResult();
    }
    
    private void updating_an_entity()
    {
        entity.UpdateName(new_name);
        entity.UpdateEmail(new_email);
        repository.Save(entity);
        retrieving_an_entity();
    }
    
    private void an_entity_exists()
    {
        an_entity();
        saving_an_entity();
    }

    private void another_entity_exists()
    {
        another_entity = new User(another_id, new_name, new_email);
        saving_another_entity();
    }    
    
    private void another_entity_with_same_id()
    {
        another_entity = new User(id, new_name, new_email);
    }
    
    private void retrieving_an_entity()
    {
        retrieved_entity = repository.Get(id).GetAwaiter().GetResult()!;
    }        
    
    private void listing_entities()
    {
        entities = repository.GetAll().GetAwaiter().GetResult();
    }     
    
    private void removing_an_entity()
    {
        repository.Remove(id).GetAwaiter().GetResult();
    }    
    
    private void the_entity_is_correct()
    {
        retrieved_entity.Id.Should().Be(entity.Id);
        retrieved_entity.Name.Should().Be(entity.Name);
        retrieved_entity.Email.Should().Be(entity.Email);
    }    
    
    private void the_entity_is_null()
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