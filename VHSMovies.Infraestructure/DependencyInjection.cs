using Microsoft.EntityFrameworkCore;
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
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ITitleRepository<Title>, TitleRepository<Title>>();
            services.AddScoped<ITitleRepository<Movie>, TitleRepository<Movie>>();
            services.AddScoped<ITitleRepository<TVShow>, TitleRepository<TVShow>>();
            services.AddScoped<ICastRepository, CastRepository>();

            return services;
        }
    }
}
