using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Infraestructure;

namespace VHSMovies.Tests.Integration.Setup
{
    public static class DbContextHelper
    {
        public static DbContextClass CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DbContextClass>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var inMemorySettings = new Dictionary<string, string>();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new DbContextClass(configuration, options);
        }
    }
}
