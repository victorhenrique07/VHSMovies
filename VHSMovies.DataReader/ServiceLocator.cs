using VHSMovies.Infraestructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Domain.Infraestructure;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Domain.Infraestructure.DataReaders;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.DataReader
{
    public static class ServiceLocator
    {
        private static Container container;

        public static void Configure(IConfiguration configuration, WebDriverManager driverManager)
        {
            container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register(() => DbContextFactory.Create(configuration), Lifestyle.Scoped);

            container.RegisterInstance(driverManager);

            container.Register<IServiceScopeFactory, SimpleInjectorServiceScopeFactory>(Lifestyle.Singleton);

            //container.Register<SyncDataService>(Lifestyle.Transient);

            container.Register(typeof(IRepository<>), typeof(Repository<>), Lifestyle.Scoped);
            container.Register(typeof(ITitleRepository<>), typeof(TitleRepository<>), Lifestyle.Scoped);
            container.Register<IPersonRepository, PersonRepository>(Lifestyle.Scoped);
            container.Register<ICastRepository, CastRepository>(Lifestyle.Scoped);
            container.Register<IGenreRepository, GenreRepository>(Lifestyle.Scoped);
            container.Register<IReviewRepository, ReviewRepository>(Lifestyle.Scoped);

            container.RegisterConditional<IHtmlReader, SeleniumManager>(Lifestyle.Singleton, c =>
                c.Consumer.ImplementationType == typeof(ImdbDataReader)
            );

            container.Collection.Register<IDataReader>(new List<Type>
            {
                typeof(ImdbDataReader)
            });

            container.Verify();
        }

        public static T GetInstance<T>() where T : class => container.GetInstance<T>();

        public static void Dispose() => container.Dispose();
    }
}
