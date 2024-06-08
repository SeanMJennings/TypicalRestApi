using NUnit.Framework;

namespace Unit.Persistence;

public partial class UserServiceSpecs
{
   [Test]
   public void can_create_entity()
   {
      Given(an_entity);
      When(saving_an_entity);
      And(retrieving_an_entity);
      Then(the_entity_is_correct);
   }   
   
   [Test]
   public void can_update_entity()
   {
      Given(an_entity_exists);
      When(updating_an_entity);
      Then(the_entity_is_correct);
   }
   
   [Test]
   public void cannot_create_an_entity_with_same_id()
   {
      Given(an_entity_exists);
      And(another_entity_with_same_id);
      When(Validating(saving_another_entity));
      Then(Informs("User with same ID already exists"));
   }
   
   [Test]
   public void can_remove_entity()
   {
      Given(an_entity_exists);
      When(removing_an_entity);
      And(retrieving_an_entity);
      Then(the_entity_is_null);
   }

   [Test]
   public void can_list_entities()
   {
      Given(an_entity_exists);
      And(another_entity_exists);
      When(listing_entities);
      Then(the_list_is_correct);
   }
}