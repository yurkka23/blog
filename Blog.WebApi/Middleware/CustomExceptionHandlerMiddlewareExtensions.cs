using Microsoft.AspNetCore.Builder;

namespace Blog.WebApi.Middleware
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this
            IApplicationBuilder builder)//to we can use middleware in pipeline
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
