using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Domain.Infraestructure;
using VHSMovies.Domain.Infraestructure.DataReaders;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.DataReader
{
    public static class ServiceLocator
    {
        private static Container container;

        public static void Configure(IConfiguration configuration, WebDriverManager driverManager)
        {
            container.Register(() => DbContextFactory.Create(), Lifestyle.Scoped);

            container.RegisterInstance(driverManager);

            container.Register(typeof(IRepository<>), typeof(Repository<>), Lifestyle.Scoped);

            container.Register<IPersonRepository, PersonRepository>(Lifestyle.Scoped);
            container.Register<ITitleRepository, TitleRepository>(Lifestyle.Scoped);

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
    }
}
