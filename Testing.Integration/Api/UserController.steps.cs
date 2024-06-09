using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Api;
using Api.Middleware;
using Application;
using Domain.Entities;
using FluentAssertions;
using Framework;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;

namespace Integration.Api;

public partial class UserControllerSpecs : DbSpecification<User>
{
    private IAmAUserService user_service = null!;
    private IntegrationWebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;
    private HttpContent content = null!;

    private readonly Guid id = Guid.NewGuid();
    private Guid returned_id;
    private readonly Guid another_id = Guid.NewGuid();
    private HttpStatusCode created_response_code;
    private HttpStatusCode response_code;
    private HttpResponseMessage the_failed_response = null!;
    private const string application_json = "application/json";
    private const string validation_error = "The name was empty!";
    private const string name = "wibble";
    private const string email = "wobble@gmail.com";    
    private readonly string invalid_name = string.Empty;
    private const string invalid_email = "oops";
    private const string new_name = "wobble";
    private const string new_email = "wibble@gmail.com";

    protected override void before_all()
    {
        user_service = Substitute.For<IAmAUserService>();
    }
    
    protected override void before_each()
    {
        base.before_each();
        content = null!;
        returned_id = default;
        the_failed_response = null!;
        user_service.ClearReceivedCalls();
        user_service.ClearSubstitute();
        factory = new IntegrationWebApplicationFactory<Program>(user_service);
        client = factory.CreateClient();
    }

    private void a_request_to_create_a_user()
    {
        create_content(name, email);
        user_service.Add(name, email).Returns(id);
    }    
    
    private void an_invalid_request_to_create_a_user()
    {
        create_content(invalid_name, invalid_email);
        user_service.Add(invalid_name, invalid_email).Throws(new ValidationException(validation_error));
    }

    private void create_content(string the_name, string the_email)
    {
        content = new StringContent($"{{\"name\":\"{the_name}\",\"email\":\"{the_email}\"}}", Encoding.UTF8, application_json);
    }

    private void a_request_to_create_another_user()
    {
        create_content(new_name, new_email);
        user_service.Add(new_name, new_email).Returns(another_id);
    }    
    
    private void a_request_to_update_the_user()
    {
        create_content(new_name, new_email);
        user_service.Get(id).Returns(new User(id, new_name, new_email));
    }

    private void a_request_to_delete_the_user()
    {
        user_service.Get(id).Returns(Task.FromResult((User?)null));
    }

    private void creating_the_user()
    {
        var response = client.PostAsync(Routes.User, content).GetAwaiter().GetResult();
        created_response_code = response.StatusCode;
        returned_id = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    }    
    
    private void creating_the_invalid_user()
    {
        the_failed_response = client.PostAsync(Routes.User, content).GetAwaiter().GetResult();
        response_code = the_failed_response.StatusCode;
    }    
    
    private void creating_another_user()
    {
        var response = client.PostAsync(Routes.User, content).GetAwaiter().GetResult();
        JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    }      
    
    private void updating_the_user()
    {
        client.PutAsync(Routes.User + $"/{id}", content).GetAwaiter().GetResult();
    }     
    
    private void deleting_the_user()
    {
        client.DeleteAsync(Routes.User + $"/{id}").GetAwaiter().GetResult();
    }    
    
    private void a_user_exists()
    {
        a_request_to_create_a_user();
        creating_the_user();
    }    
    
    private void another_user_exists()
    {
        a_request_to_create_another_user();
        creating_another_user();
    }

    private void requesting_the_user()
    {
        user_service.Get(id).Returns(new User(id, name, email));
        var response = client.GetAsync(Routes.User + $"/{id}").GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }    
    
    private void requesting_the_deleted_user()
    {
        user_service.Get(id).Returns((User?)null);
        var response = client.GetAsync(Routes.User + $"/{id}").GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }    
    
    private void requesting_the_updated_user()
    {
        user_service.Get(id).Returns(new User(id, new_name, new_email));
        var response = client.GetAsync(Routes.User + $"/{id}").GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }
    
    private void listing_the_users()
    {
        user_service.GetAll().Returns(new List<User>{new (id, name, email), new (another_id, new_name, new_email)});
        var response = client.GetAsync(Routes.User).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        content = response.Content;
    }

    private void the_user_is_created()
    {
        var user = JsonConvert.DeserializeObject<User>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        created_response_code.Should().Be(HttpStatusCode.Created);
        user!.Id.Should().Be(returned_id);
        user.Name.ToString().Should().Be(name);
        user.Email.ToString().Should().Be(email);
    }    
    
    private void the_user_is_informed_of_validation_errors()
    {
        response_code.Should().Be(HttpStatusCode.BadRequest);
        var problem = JsonConvert.DeserializeObject<ExceptionMiddleware.Problem>(the_failed_response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        problem!.Message.Should().Be("The request did not validate correctly");
        problem.Errors[0].Should().Be(validation_error);
    }    
    
    private void the_user_is_updated()
    {
        var user = JsonConvert.DeserializeObject<User>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        response_code.Should().Be(HttpStatusCode.OK);
        user!.Id.Should().Be(id);
        user.Name.ToString().Should().Be(new_name);
        user.Email.ToString().Should().Be(new_email);
    }    
    
    private void the_users_are_listed()
    {
        var users = JsonConvert.DeserializeObject<IReadOnlyList<User>>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        response_code.Should().Be(HttpStatusCode.OK);
        users!.Count.Should().Be(2);
        users[0].Id.Should().Be(id);
        users[0].Name.ToString().Should().Be(name);
        users[0].Email.ToString().Should().Be(email);        
        users[1].Id.Should().Be(another_id);
        users[1].Name.ToString().Should().Be(new_name);
        users[1].Email.ToString().Should().Be(new_email);
    }    
    
    private void the_user_is_not_found()
    {
        response_code.Should().Be(HttpStatusCode.NotFound);
    }
}