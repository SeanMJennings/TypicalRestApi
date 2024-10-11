using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;

namespace Api.Middleware;

public class ExceptionMiddleware(RequestDelegate NextRequest)
{
    internal record Problem(string Message, params string[] Errors);

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await NextRequest(httpContext);
        }
        catch (Exception e)
        {
            var handled = await HandleException(httpContext, e);
            if (!handled) throw;
        }
    }

    private static async Task<bool> HandleException(HttpContext httpContext, Exception exception)
    {
        if (exception is not ValidationException) return false;
        
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var problem = new Problem("The request did not validate correctly", exception.Message);
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(problem));
        return true;

    }
}