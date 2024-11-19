using DemoWebApi.Application.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DemoWebApi.Application.Behaviors;

public class ValidatorBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }
        
        var typeName = request.GetGenericTypeName();
        logger.LogInformation("Validating request {RequestType}", typeName);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count <= 0)
        {
            return await next();
        }
        
        logger.LogWarning(
            "Validation errors - {RequestType} - Request: {@Request} - Errors: {@ValidationErrors}", typeName,
            request, failures);
                
        throw new ValidationException(failures);
    }
}