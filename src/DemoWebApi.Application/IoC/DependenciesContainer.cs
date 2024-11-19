using DemoWebApi.Application.Behaviors;
using DemoWebApi.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebApi.Application.IoC;

public static class DependenciesContainer
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.Lifetime = ServiceLifetime.Scoped;
            cfg.RegisterServicesFromAssemblyContaining<CreateDemoTaskCommand>();
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddValidatorsFromAssemblyContaining<CreateDemoTaskCommand>();
        
        return services;
    }
}