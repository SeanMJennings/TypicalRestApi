using NUnit.Framework;

namespace Integration.Api;

public partial class UserControllerSpecs
{
    [Test]
    public void can_create_user()
    {
        Given(a_request_to_create_a_user);
        When(creating_the_user);
        And(requesting_the_user);
        Then(the_user_is_created);
    }    
    
    [Test]
    public void informs_of_validation_errors()
    {
        Given(an_invalid_request_to_create_a_user);
        When(creating_the_invalid_user);
        Then(the_user_is_informed_of_validation_errors);
    }
    
    [Test]
    public void can_update_user()
    {
        Given(a_user_exists);
        And(a_request_to_update_the_user);
        When(updating_the_user);
        And(requesting_the_updated_user);
        Then(the_user_is_updated);
    }    
    
    [Test]
    public void can_delete_user()
    {
        Given(a_user_exists);
        And(a_request_to_delete_the_user);
        When(deleting_the_user);
        And(requesting_the_deleted_user);
        Then(the_user_is_not_found);
    }
    
    [Test]
    public void can_list_users()
    {
        Given(a_user_exists);
        And(another_user_exists);
        When(listing_the_users);
        Then(the_users_are_listed);
    }
}