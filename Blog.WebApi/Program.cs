using Blog.Application;
using Blog.Application.Common.Mappings;
using Blog.Application.Interfaces;
using Blog.Persistence;
using Blog.WebApi.Middleware;
using Blog.WebApi.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//add services to the cointainer
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserService, UserService>();//
builder.Services.AddHttpContextAccessor();//
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});//
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });//

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IBlogDbContext).Assembly));
});

builder.Services.AddApplication();
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

//builder.Services.AddSwaggerGen();

/////////////////
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.RoutePrefix = String.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog v1");
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

//app.UseAuthorization();

app.UseCustomExceptionHandler();

app.UseRouting();


app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();




////////////////////////////////////////////////////////////////////////////////////////////////////////////
//using Infrastructure.EntityFramework;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Blog
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var host = CreateHostBuilder(args).Build();

//            using (var scope = host.Services.CreateScope())
//            {
//                var serviceProvider = scope.ServiceProvider;
//                try
//                {
//                    var context = serviceProvider.GetRequiredService<BlogDbContext>();
//                    //DbInitializer.Initialize(context);
//                }
//                catch(Exception e)
//                {
//                    Console.WriteLine(e.Message);
//                }
                

//            }

//            host.Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}