using FluentAssertions;
using Framework;
using Persistence;

namespace Unit.Persistence;

public partial class InMemoryRepositorySpecs : DbSpecification<InMemoryRepositorySpecs.Fake>
{
    public class Fake(Guid id, string wibble) : Aggregate(id)
    {
        public string Wibble { get; private set; } = wibble;
        public void UpdateWibble(string wibble) => Wibble = wibble;
    }
    
    private readonly Guid id = Guid.NewGuid();
    private readonly Guid another_id = Guid.NewGuid();
    private Fake entity = null!;
    private Fake another_entity = null!;
    private Fake retrieved_entity = null!;
    private IReadOnlyList<Fake> entities = null!;
    private IAmARepository<Fake> repository = null!;

    private const string wibble = "wibble";
    private const string wobble = "wobble";

    protected override void before_each()
    {
        base.before_each();
        entity = null!;
        entities = null!;
        another_entity = null!;
        retrieved_entity = null!;
        repository = new InMemoryRepository<Fake>(database_context);
    }
    
    private void an_entity()
    {
        entity = new Fake(id, wibble);
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
        entity.UpdateWibble(wobble);
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
        another_entity = new Fake(another_id, wobble);
        saving_another_entity();
    }    
    
    private void another_entity_with_same_id()
    {
        another_entity = new Fake(id, wobble);
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
        retrieved_entity.Wibble.Should().Be(entity.Wibble);
    }    
    
    private void the_entity_is_null()
    {
        retrieved_entity.Should().Be(null);
    }    
    
    private void the_list_is_correct()
    {
        entities.Count.Should().Be(2);
        entities[0].Id.Should().Be(id);
        entities[0].Wibble.Should().Be(wibble);
        entities[1].Id.Should().Be(another_id);
        entities[1].Wibble.ToString().Should().Be(wobble);
    }
}