using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Api.Extensions;

public static class ResultExtension
{
    public static ProblemDetails ToProblemDetails<T>(this Result<T> result, HttpRequest httpRequest)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = $"{httpRequest.Method} {httpRequest.Path}",
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        List<string> validationErrors = [];
        foreach (var error in result.Errors)
        {
            validationErrors.Add(error.Message);
        }

        problemDetails.Extensions.Add("errors", validationErrors);
        return problemDetails;
    }
}