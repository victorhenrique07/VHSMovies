using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string DATABASE_HOST = Environment.GetEnvironmentVariable("DATABASE_HOST", EnvironmentVariableTarget.Machine);
            string DATABASE_USERNAME = Environment.GetEnvironmentVariable("DATABASE_USERNAME", EnvironmentVariableTarget.Machine);
            string DATABASE_PASSWORD = Environment.GetEnvironmentVariable("DATABASE_PASSWORD", EnvironmentVariableTarget.Machine);
            string DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME", EnvironmentVariableTarget.Machine);
            string DATABASE_PORT = Environment.GetEnvironmentVariable("DATABASE_PORT", EnvironmentVariableTarget.Machine);

            string connectionString = $"Server={DATABASE_HOST};Port={DATABASE_PORT};Database={DATABASE_NAME};Uid={DATABASE_USERNAME};Pwd={DATABASE_PASSWORD};";

            services.AddDbContext<DbContextClass>(options =>
                options.UseNpgsql(connectionString)
                   .EnableSensitiveDataLogging()
                   .LogTo(Console.WriteLine, LogLevel.Information));

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<ITitleGenreRepository, TitleGenreRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRecommendedTitlesRepository, RecommendedTitlesRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();

            return services;
        }
    }
}
