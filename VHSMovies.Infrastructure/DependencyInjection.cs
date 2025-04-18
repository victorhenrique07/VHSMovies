﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddDbContext<DbContextClass>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging() // Habilita log detalhado
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ICastRepository, CastRepository>();
            services.AddScoped<ITitleGenreRepository, TitleGenreRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRecommendedTitlesRepository, RecommendedTitlesRepository>();
            services.AddScoped<ITitleRepository<Title>, TitleRepository<Title>>();
            services.AddScoped<ITitleRepository<Movie>, TitleRepository<Movie>>();
            services.AddScoped<ITitleRepository<TVShow>, TitleRepository<TVShow>>();

            return services;
        }
    }
}
