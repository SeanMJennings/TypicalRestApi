using Domain.Entities;
using FluentAssertions;
using Framework;

namespace Unit.Domain;

public partial class UserSpecs : Specification
{
    private Guid id;
    private string name = null!;
    private string email = null!;
    private User user = null!;

    private const string invalid_email = "wibble";
    private const string valid_email = "wibble@wobble.com";
    private const string valid_name = "Jackie Chan";

    protected override void before_each()
    {
        base.before_each();
        id = Guid.NewGuid();
        name = null!;
        email = null!;
        user = null!;
    }

    private void valid_inputs()
    {
        name = valid_name;
        email = valid_email;
    }

    private void a_null_user_name()
    {
        name = null!;
    }    
    
    private void an_empty_user_name()
    {
        name = string.Empty;
    }    
    
    private void a_null_email()
    {
        email = null!;
    }    
    
    private void an_empty_email()
    {
        email = string.Empty;
    }    
    
    private void an_invalid_email()
    {
        email = invalid_email;
    }

    private void creating_a_user()
    {
        user = new User(id, name, email);
    }    
    
    private void the_user_is_created()
    {
        user.Id.Should().Be(id);
        user.Name.ToString().Should().Be(valid_name);
        user.Email.ToString().Should().Be(valid_email);
    }
}