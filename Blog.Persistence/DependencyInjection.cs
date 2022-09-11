using Blog.Persistence.EntityContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using Blog.Application.Interfaces;

namespace Blog.Persistence
{
    public static class DependencyInjection
    {
        //add context DB and register it
        public static IServiceCollection AddPersistance(this IServiceCollection services , IConfiguration configuration)//add context DB and register it
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IBlogDbContext>(provider => provider.GetService<BlogDbContext>());
            return services;
        }
    }
}
