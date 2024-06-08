namespace Api.Requests;

public class UserPayload(string name, string email)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
}