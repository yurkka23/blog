
//using Blog.Application;
//using Blog.Application.Common.Mappings;
//using Blog.Application.Interfaces;
//using Blog.Persistence;
//using Blog.WebApi.Middleware;
//using FluentValidation;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//using System.Reflection;

//namespace Blog
//{
//    public class Startup
//    {
//        public IConfiguration Configuration { get; }
//        public Startup(IConfiguration configuration) => Configuration = configuration;

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddAutoMapper(config =>
//            {
//                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
//                config.AddProfile(new AssemblyMappingProfile(typeof(IBlogDbContext).Assembly));
//            });
           
//            services.AddApplication();
//            services.AddPersistance(Configuration);
//            services.AddControllers();
//            services.AddCors(options =>
//            {
//                options.AddPolicy("AllowAll", policy =>
//                {
//                    policy.AllowAnyHeader();
//                    policy.AllowAnyMethod();
//                    policy.AllowAnyOrigin();
//                });
//            });

//            services.AddSwaggerGen();
            
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
                
//                app.UseSwagger();
//                app.UseSwaggerUI(c => {
//                    c.RoutePrefix = String.Empty;
//                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog v1");
//                });
//            }
            

//            app.UseCustomExceptionHandler();

//            app.UseRouting();

//            app.UseHttpsRedirection();

//            app.UseCors("AllowAll");

//            //app.UseAuthorization();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }
//}