using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Infrastructure;
using VHSMovies.Infrastructure.Redis;
using VHSMovies.Infrastructure.Services;

namespace VHSMovies.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment == null || environment.IsDevelopment())
            {
                services.AddDbContext<DbContextClass>(options =>
                    options.UseSqlite("DataSource=:memory:")
                           .EnableSensitiveDataLogging()
                           .LogTo(Console.WriteLine, LogLevel.Information));
            }
            else
            {
                DbConfigurationManager manager = DbConfigurationManager.Instance;

                string DATABASE_HOST = manager.GetConfigurationValue("DATABASE_HOST");
                string DATABASE_USERNAME = manager.GetConfigurationValue("DATABASE_USERNAME");
                string DATABASE_PASSWORD = manager.GetConfigurationValue("DATABASE_PASSWORD");
                string DATABASE_NAME = manager.GetConfigurationValue("DATABASE_NAME");
                string DATABASE_PORT = manager.GetConfigurationValue("DATABASE_PORT");

                string connectionString = $"Server={DATABASE_HOST};Port={DATABASE_PORT};Database={DATABASE_NAME};Uid={DATABASE_USERNAME};Pwd={DATABASE_PASSWORD};";

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("The database connection string was not defined.");

                services.AddDbContext<DbContextClass>(options =>
                    options.UseNpgsql(connectionString)
                       .EnableSensitiveDataLogging()
                       .LogTo(Console.WriteLine, LogLevel.Information));

            }

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRecommendedTitlesRepository, RecommendedTitlesRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration["REDIS_CONNECTION_STRING"]
            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING", EnvironmentVariableTarget.Process)
            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING", EnvironmentVariableTarget.Machine)
            ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(redisConnectionString))
                throw new InvalidOperationException("The redis connection string was not defined.");

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnectionString)
            );

            services.AddScoped<IRedisRepository, RedisRepository>();

            return services;
        }

        public static IServiceCollection AddTMDbClient(this IServiceCollection services, IConfiguration configuration)
        {
            var apiKey = configuration["TMDB_API_KEY"]
            ?? Environment.GetEnvironmentVariable("TMDB_API_KEY", EnvironmentVariableTarget.Process)
            ?? Environment.GetEnvironmentVariable("TMDB_API_KEY", EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable("TMDB_API_KEY", EnvironmentVariableTarget.Machine)
            ?? Environment.GetEnvironmentVariable("TMDB_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("The TMDb API key was not defined.");

            services.AddHttpClient<ITMDbService, TMDbService>(client =>
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                client.DefaultRequestHeaders.Add("accept", "application/json");
            });

            return services;
        }
    }
}
