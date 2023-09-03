using LocalS3API.Service;

namespace LocalS3API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServices();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddResponseCaching();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.AddMiddleware();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseResponseCaching();

            app.Run();
        }
    }
}