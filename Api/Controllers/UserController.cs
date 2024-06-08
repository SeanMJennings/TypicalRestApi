using Api.Requests;
using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(Routes.User)]
public class UserController(IAmAUserService UserService) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<User>> GetUsers()
    {
        return await UserService.GetAll();
    }    
    
    [HttpGet("{id:guid}")]
    public async Task<User?> GetUser(Guid id)
    {
        return await UserService.Get(id);
    }    
    
    [HttpPost]
    public async Task<Guid> CreateUser([FromBody] UserPayload payload)
    {
        return await UserService.Add(payload.Name, payload.Email);
    }    
    
    [HttpPut("{id:guid}")]
    public async Task UpdateUser(Guid id, [FromBody] UserPayload payload)
    {
        await UserService.Update(id, payload.Name, payload.Email);
    }    
    
    [HttpDelete("{id:guid}")]
    public async Task UpdateUser(Guid id)
    {
        await UserService.Remove(id);
    }
}