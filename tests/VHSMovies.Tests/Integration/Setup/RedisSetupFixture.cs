using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Moq;

using StackExchange.Redis;

using VHSMovies.Infrastructure.Redis;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Tests.Integration.Setup
{
    public class RedisSetupFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var redisDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IRedisRepository));

                if (redisDescriptor != null)
                    services.Remove(redisDescriptor);

                var multiplexerDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IConnectionMultiplexer));

                if (multiplexerDescriptor != null)
                    services.Remove(multiplexerDescriptor);

                services.AddSingleton<IConnectionMultiplexer>(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var redisConnectionString = "localhost:6379,password=redis,defaultDatabase=0"; ;

                    return ConnectionMultiplexer.Connect(redisConnectionString);
                });

                services.AddSingleton<IRedisRepository, RedisRepository>();
            });
        }
    }
}
