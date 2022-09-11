using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Blog.Application.Common.Behaviors;

namespace Blog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());//add register for mediatR

            //services.AddScoped<IValidator<CreateArticleCommand>, CreateArticleCommandValidator>();
            //services.AddScoped<IValidator<DeleteArticleCommand>, DeleteArticleCommandValidator>();
            //services.AddScoped<IValidator<VerifyArticleCommand>, VerifyArticleCommandValidator>();
            // services.AddScoped<IValidator<UpdateArticleCommand>, UpdateArticleCommandValidator>();
            
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });//add all validators from assembly
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));//register pipeline behavior
            return services;
        }
    }
}
