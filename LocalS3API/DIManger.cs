using LocalS3API.Middleware;
using LocalS3API.Service;

namespace LocalS3API
{
    public static class DIManger
    {
        public static void AddServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<S3FileService>();
        }

        public static void AddMiddleware(this WebApplication webApplication)
        {
            webApplication.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}