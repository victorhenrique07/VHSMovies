using Microsoft.EntityFrameworkCore;
using VHSMovies.Infraestructure;

namespace VHSMovies.Api.Settings
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using DbContextClass dbContext = 
                scope.ServiceProvider.GetRequiredService<DbContextClass>();

            dbContext.Database.Migrate();
        }
    }
}
