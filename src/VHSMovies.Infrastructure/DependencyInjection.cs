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
using VHSMovies.Infrastructure;

namespace VHSMovies.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            DbConfigurationManager manager = DbConfigurationManager.Instance;

            string DATABASE_HOST = manager.GetConfigurationValue("DATABASE_HOST");
            string DATABASE_USERNAME = manager.GetConfigurationValue("DATABASE_USERNAME");
            string DATABASE_PASSWORD = manager.GetConfigurationValue("DATABASE_PASSWORD");
            string DATABASE_NAME = manager.GetConfigurationValue("DATABASE_NAME");
            string DATABASE_PORT = manager.GetConfigurationValue("DATABASE_PORT");

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
