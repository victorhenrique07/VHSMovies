using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

using Moq;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure;

namespace VHSMovies.Tests.Integration.Setup
{
    public class DatabaseSetupFixture
    {
        private List<Person> people;
        private List<Title> titles;
        private List<Genre> genres;

        private IWebHostEnvironment env;

        public DbContextClass CreateInMemoryDbContext()
        {
            env = new Mock<IWebHostEnvironment>().Object;

            var options = new DbContextOptionsBuilder<DbContextClass>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var inMemorySettings = new Dictionary<string, string>();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new DbContextClass(configuration, env, options);
        }

        public void Dispose(DbContextClass Context)
        {
            Context.Dispose();
        }
    }
}