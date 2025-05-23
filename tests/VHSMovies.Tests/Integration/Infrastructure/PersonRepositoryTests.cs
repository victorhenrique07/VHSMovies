using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Infrastructure
{
    [Collection("DatabaseCollection")]
    public class PersonRepositoryTests
    {
        private readonly DatabaseSetupFixture databaseFixture;

        public PersonRepositoryTests(DatabaseSetupFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
        }

        [Theory]
        [InlineData(PersonRole.Actor, 4)]
        [InlineData(PersonRole.Director, 3)]
        [InlineData(PersonRole.Writer, 3)]
        [InlineData(PersonRole.None, 10)]
        public async Task ShouldReturnAllPerson(PersonRole role, int expectedCountResult)
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            IReadOnlyCollection<Person> people = new List<Person>();
            var repository = new PersonRepository(context);

            // Act
            people = await repository.GetAllPerson(role);

            // Assert
            people.Should().NotBeEmpty();
            people.Should().HaveCount(expectedCountResult);
        }

        [Theory]
        [InlineData(1, "Tim Robbins")]
        [InlineData(2, "Christian Bale")]
        [InlineData(3, "Bryan Cranston")]
        [InlineData(4, "Emilia Clarke")]
        public async Task ShouldReturnPersonById(int id, string expectedName)
        {
            //Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            var repository = new PersonRepository(context);

            // Act
            Person person = await repository.GetPersonById(id);

            // Assert
            person.Should().NotBeNull();
            person.Name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(1, "Tim Robbins")]
        [InlineData(2, "Christian Bale")]
        [InlineData(3, "Bryan Cranston")]
        [InlineData(4, "Emilia Clarke")]
        public async Task ShouldReturnPersonByRole(int id, string expectedName)
        {
            //Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            var repository = new PersonRepository(context);

            // Act
            Person person = await repository.GetPersonById(id);

            // Assert
            person.Should().NotBeNull();
            person.Name.Should().Be(expectedName);
        }

        [Fact]
        public async Task ShouldSavePersonList()
        {
            // Arrange
            using var context = databaseFixture.CreateInMemoryDbContext();

            var repository = new PersonRepository(context);
            IReadOnlyCollection<Person> personList = new List<Person>
            {
                new Person("Tim Robbins"),
                new Person("Christian Bale"),
                new Person("Bryan Cranston"),
                new Person("Emilia Clarke"),
            };

            // Act
            await repository.RegisterListAsync(personList);
            await repository.SaveChangesAsync();

            var people = await repository.GetAllPerson(PersonRole.None);

            // Assert
            people.Should().NotBeNull();
            people.Should().HaveCount(4);
            people.Should().BeEquivalentTo(personList);
        }
    }
}
