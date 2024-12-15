using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace VHSMovies.DataReader
{
    public class SimpleInjectorServiceScopeFactory : IServiceScopeFactory
    {
        private readonly Container _container;

        public SimpleInjectorServiceScopeFactory(Container container)
        {
            _container = container;
        }

        public IServiceScope CreateScope()
        {
            return new SimpleInjectorServiceScope(_container);
        }
    }

    public class SimpleInjectorServiceScope : IServiceScope
    {
        private readonly Scope _scope;

        public SimpleInjectorServiceScope(Container container)
        {
            _scope = AsyncScopedLifestyle.BeginScope(container);
            ServiceProvider = new SimpleInjectorServiceProvider(container);
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }

    public class SimpleInjectorServiceProvider : IServiceProvider
    {
        private readonly Container _container;

        public SimpleInjectorServiceProvider(Container container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }
    }
}