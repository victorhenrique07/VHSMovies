using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Mediator.Implementation
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public SimpleMediator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();

            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            dynamic handler = scope.ServiceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {request.GetType().Name} not found.");
            }

            return await (Task<TResponse>)handlerType.GetMethod("Handle")
                .Invoke(handler, new object[] { request, cancellationToken });
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
            var handlers = _provider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                await (Task)handlerType.GetMethod("Handle")
                    .Invoke(handler, new object[] { notification, cancellationToken });
            }
        }
    }
}
