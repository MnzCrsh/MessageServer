using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MessageServer.Presentation.ControllerFilters;

public class ExceptionInterceptor : Attribute,IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var error = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occured while processing your request",
            Type = "https://tools.ietf.org/html/rfc7231"
        };

        context.Result = new ObjectResult(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        return Task.CompletedTask;
    }
}