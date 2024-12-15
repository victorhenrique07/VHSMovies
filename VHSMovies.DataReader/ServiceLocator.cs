using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Domain.Infraestructure;
using VHSMovies.Domain.Infraestructure.DataReaders;
using VHSMovies.Domain.Infraestructure.Services;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.DataReader
{
    public static class ServiceLocator
    {
        private static Container container;

        public static void Configure(IConfiguration configuration, WebDriverManager driverManager)
        {
            container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register(() => DbContextFactory.Create(configuration), Lifestyle.Singleton);

            container.RegisterInstance(driverManager);

            container.Register<SyncDataService>(Lifestyle.Transient);
            container.Register(typeof(IRepository<>), typeof(Repository<>), Lifestyle.Singleton);
            container.Register(typeof(ITitleRepository<>), typeof(TitleRepository<>), Lifestyle.Singleton);

            container.Register<IPersonRepository, PersonRepository>(Lifestyle.Singleton);


            container.RegisterConditional<IHtmlReader, SeleniumManager>(Lifestyle.Singleton, c =>
                c.Consumer.ImplementationType == typeof(ImdbDataReader)
            );

            container.Collection.Register<IDataReader>(new List<Type>
            {
                typeof(ImdbDataReader)
            });

            // Verifica se as configurações estão corretas
            container.Verify();
        }

        public static T GetInstance<T>() where T : class => container.GetInstance<T>();

        public static void Dispose() => container.Dispose();
    }
}
