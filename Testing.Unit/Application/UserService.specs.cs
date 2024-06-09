using Framework;
using NUnit.Framework;

namespace Unit.Application;

public partial class UserServiceSpecs
{
   [Test]
   public void can_create_user()
   {
      Given(user_details);
      When(adding_a_user);
      And(retrieving_a_user);
      Then(the_user_is_correct);
   }   
   
   [Test]
   public void can_update_user()
   {
      Given(a_user_exists);
      When(updating_a_user);
      Then(the_user_is_updated);
   }
   
   [Test]
   public void cannot_create_a_user_with_same_email()
   {
      Given(a_user_exists);
      When(Validating(saving_another_user_with_same_email));
      Then(Informs("User with same email already exists"));
   }
   
   [Test]
   public void can_remove_user()
   {
      Given(a_user_exists);
      When(removing_an_user);
      And(retrieving_a_user);
      Then(the_user_is_null);
   }

   [Test]
   public void can_list_users()
   {
      Given(a_user_exists);
      And(another_user_exists);
      When(listing_entities);
      Then(the_list_is_correct);
   }
}