using JobOffersManager.API.Services;
using JobOffersManager.API.Data;
using JobOffersManager.API.Middleware;
using Microsoft.EntityFrameworkCore;

namespace JobOffersManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Services
            builder.Services.AddScoped<IJobOffersService, JobOffersService>();

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Ensure database is created
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            // Swagger only in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Global exception handling middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
