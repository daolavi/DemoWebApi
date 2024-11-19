using DemoWebApi.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DemoWebApi.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling request {RequestName} ({@Request})", request.GetGenericTypeName(), request);
        var response = await next();
        logger.LogInformation("Request {RequestName} handled - response: {@Response}", request.GetGenericTypeName(), response);

        return response;
    }
}