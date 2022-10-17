using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Blog.Application.Common.Behaviors;
using Microsoft.Extensions.Configuration;

namespace Blog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());//add register for mediatR
 
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });//add all validators from assembly

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));//register pipeline behavior
        return services;
    }
}
