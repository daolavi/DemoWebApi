using DemoWebApi.Application.Behaviors;
using DemoWebApi.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebApi.Application.IoC;

public static class DependenciesContainer
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.Lifetime = ServiceLifetime.Scoped;
            cfg.RegisterServicesFromAssemblyContaining<GetDemoTaskByIdQuery>();
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        
        return services;
    }
}