using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using VHSMovies.Application.Mappers;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Extensions;

namespace VHSMovies.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PersonMapper));

            services.AddSimpleMediator();

            return services;
        }
    }
}
