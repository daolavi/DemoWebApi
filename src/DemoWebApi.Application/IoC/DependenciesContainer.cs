using System.Reflection;
using DemoWebApi.Application.Behaviors;
using DemoWebApi.Application.Commands;
using DemoWebApi.Application.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebApi.Application.IoC;

public static class DependenciesContainer
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}