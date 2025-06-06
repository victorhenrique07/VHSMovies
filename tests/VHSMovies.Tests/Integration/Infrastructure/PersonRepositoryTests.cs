﻿using System;
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
        [InlineData(PersonRole.Actor, 5)]
        [InlineData(PersonRole.Director, 8)]
        [InlineData(PersonRole.Writer, 9)]
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
                new Person("Tim Robbins") { IMDB_Id = "tt1"},
                new Person("Christian Bale") { IMDB_Id = "tt2"},
                new Person("Bryan Cranston") { IMDB_Id = "tt3"},
                new Person("Emilia Clarke") { IMDB_Id = "tt4"},
            };

            // Act
            await repository.RegisterListAsync(personList);
            await repository.SaveChangesAsync();

            var people = await repository.GetAllPerson();

            // Assert
            people.Should().NotBeNull();
            people.Should().HaveCount(4);
            people.Should().BeEquivalentTo(personList);
        }
    }
}
