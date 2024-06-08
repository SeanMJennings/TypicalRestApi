using System.Net;
using System.Text;
using Api;
using Application;
using Domain.Entities;
using FluentAssertions;
using Framework;
using Newtonsoft.Json;
using Persistence;

namespace Integration.Api;

public partial class UserControllerSpecs : DbSpecification<User>
{
    private IAmARepository<User> repository = null!;
    private IAmAUserService user_service = null!;
    private IntegrationWebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;
    private HttpContent content = null!;

    private Guid id;
    private Guid another_id;
    private HttpStatusCode response_code;
    private HttpResponseMessage the_failed_response = null!;
    private const string application_json = "application/json";
    private const string name = "wibble";
    private const string email = "wobble@gmail.com";    
    private readonly string invalid_name = string.Empty;
    private const string invalid_email = "oops";
    private const string new_name = "wobble";
    private const string new_email = "wibble@gmail.com";
    
    protected override void before_each()
    {
        base.before_each();
        content = null!;
        id = default;
        another_id = default;
        the_failed_response = null!;
        repository = new InMemoryRepository<User>(database_context);
        user_service = new UserService(repository);
        factory = new IntegrationWebApplicationFactory<Program>(user_service);
        client = factory.CreateClient();
    }

    private void a_request_to_create_a_user()
    {
        CreateContent(name, email);
    }    
    
    private void an_invalid_request_to_create_a_user()
    {
        CreateContent(invalid_name, invalid_email);
    }

    private void CreateContent(string the_name, string the_email)
    {
        content = new StringContent($"{{\"name\":\"{the_name}\",\"email\":\"{the_email}\"}}", Encoding.UTF8, application_json);
    }

    private void a_request_to_create_another_user()
    {
        CreateContent(new_name, new_email);
    }    
    
    private void a_request_to_update_the_user()
    {
        CreateContent(new_name, new_email);
    }    
    
    private void a_request_to_delete_the_user(){}

    private void creating_the_user()
    {
        var response = client.PostAsync(Routes.User, content).GetAwaiter().GetResult();
        response_code = response.StatusCode;
        id = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    }    
    
    private void creating_the_invalid_user()
    {
        the_failed_response = client.PostAsync(Routes.User, content).GetAwaiter().GetResult();
        response_code = the_failed_response.StatusCode;
    }    
    
    private void creating_another_user()
    {
        var response = client.PostAsync(Routes.User, content).GetAwaiter().GetResult();
        another_id = JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
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
        content = client.GetAsync(Routes.User + $"/{id}").GetAwaiter().GetResult().Content;
    }    
    
    private void listing_the_users()
    {
        content = client.GetAsync(Routes.User).GetAwaiter().GetResult().Content;
    }

    private void the_user_is_created()
    {
        var user = JsonConvert.DeserializeObject<User>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        response_code.Should().Be(HttpStatusCode.OK);
        user!.Id.Should().Be(id);
        user.Name.ToString().Should().Be(name);
        user.Email.ToString().Should().Be(email);
    }    
    
    private void the_user_is_informed_of_validation_errors()
    {
        response_code.Should().Be(HttpStatusCode.BadRequest);
        var problem = JsonConvert.DeserializeObject<ExceptionMiddleware.Problem>(the_failed_response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        problem!.Message.Should().Be("The request did not validate correctly");
        problem.Errors[0].Should().Be("User name cannot be empty");
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
        var user = JsonConvert.DeserializeObject<User>(content.ReadAsStringAsync().GetAwaiter().GetResult());
        user.Should().BeNull();
    }
}