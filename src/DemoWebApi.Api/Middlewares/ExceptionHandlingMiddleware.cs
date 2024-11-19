using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Title = "One or more validation errors occurred.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            List<string> validationErrors = [];
            foreach (var error in exception.Errors)
            {
                validationErrors.Add(error.ErrorMessage);
            }

            problemDetails.Extensions.Add("errors", validationErrors);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Title = exception.Message
            };
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}