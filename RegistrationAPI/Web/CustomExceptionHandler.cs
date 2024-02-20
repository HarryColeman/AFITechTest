using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RegistrationAPI.Web;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _handlers;

    public CustomExceptionHandler()
    {
        _handlers = new()
        {
            { typeof(ValidationException), HandleValidationException }
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_handlers.TryGetValue(exceptionType, out Func<HttpContext, Exception, Task>? value))
        {
            await value.Invoke(httpContext, exception);
            return true;
        }

        return false;
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = ex as ValidationException;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var mappedErrors = exception?
            .Errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(fg => fg.Key, fg => fg.ToArray())
            ?? []; // We handle the null case by still returning a 400, just no errors.

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(mappedErrors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
        });
    }
}
