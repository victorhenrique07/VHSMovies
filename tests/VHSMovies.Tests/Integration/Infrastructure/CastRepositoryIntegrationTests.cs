using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Infrastructure
{
    [Collection("DatabaseCollection")]
    public class CastRepositoryIntegrationTests
    {
        private readonly DatabaseSetupFixture _fixture;

        public CastRepositoryIntegrationTests(DatabaseSetupFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldReturnAllCast()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            IReadOnlyCollection<Cast> casts = new List<Cast>();
            var repository = new CastRepository(context);

            // Act
            casts = await repository.GetAll();

            // Assert
            casts.Should().NotBeEmpty();
            casts.Should().HaveCount(10);
        }

        [Theory]
        [InlineData(1, "Tim Robbins", 1297469)]
        [InlineData(2, "Frank Darabont", 1297469)]
        [InlineData(3, "Stephen King", 1297469)]
        [InlineData(4, "Christian Bale", 1489427)]
        [InlineData(5, "Christopher Nolan", 1489427)]
        public async Task ShoulReturnCastById(int castId, string expectedPersonName, int expectedTitleId)
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            var repository = new CastRepository(context);

            // Act
            Cast cast = await repository.GetByIdAsync(castId);

            // Assert
            cast.Should().NotBeNull();
            cast.Person.Name.Should().Be(expectedPersonName);
            cast.TitleId.Should().Be(expectedTitleId);
            cast.Title.Id.Should().Be(expectedTitleId);
        }

        [Theory]
        [InlineData(PersonRole.Actor, 4)]
        [InlineData(PersonRole.Writer, 3)]
        [InlineData(PersonRole.Director, 3)]
        public async Task ShouldReturnTaskByPersonRole(PersonRole expectedRole, int expectedCastsCount)
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            var repository = new CastRepository(context);

            // Act
            IReadOnlyCollection<Cast> casts = await repository.GetCastsByPersonRole(expectedRole);

            // Assert
            casts.Should().NotBeNullOrEmpty();
            casts.Should().HaveCount(expectedCastsCount);
            casts.Should().AllSatisfy(cast =>
            {
                cast.Role.Should().Be(expectedRole);
                cast.Person.Name.Should().NotBeNullOrEmpty();
                cast.Title.Should().NotBeNull();
            });
        }

        [Theory]
        [InlineData(1297469, new[] { 1, 5, 8 })]
        [InlineData(1489427, new[] { 2, 6, 9 })]
        [InlineData(1519096, new[] { 3, 7 })]
        [InlineData(1522910, new[] { 4, 10 })]
        public async Task ShouldReturnCastByTitleId(int titleId, int[] peopleIds)
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            var repository = new CastRepository(context);

            // Act
            IReadOnlyCollection<Cast> casts = await repository.GetAllCastByTitleAsync(titleId);

            // Assert
            casts.Should().NotBeNullOrEmpty();
            casts.Should().AllSatisfy(cast =>
            {
                cast.TitleId.Should().Be(titleId);
                cast.Title.Should().NotBeNull();
                cast.Person.Should().NotBeNull();
                cast.Person.Id.Should().BeOneOf(peopleIds);
            });
        }

        [Fact]
        public async Task ShouldSaveCastList()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            context.Titles.AddRange(_fixture.GetTitlesList());
            context.People.AddRange(_fixture.GetPersonList());

            var repository = new CastRepository(context);
            IReadOnlyCollection<Cast> casts = new List<Cast>()
            {
                new Cast
                {
                    Id = 1,
                    TitleId = 1297469,
                    PersonId = 1, // Tim Robbins
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 4,
                    TitleId = 1489427,
                    PersonId = 2, // Christian Bale
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 7,
                    TitleId = 1519096,
                    PersonId = 3, // Bryan Cranston
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 9,
                    TitleId = 1522910,
                    PersonId = 4, // Emilia Clarke
                    Role = PersonRole.Actor
                },
            };

            // Act
            await repository.RegisterListAsync(casts);
            await repository.SaveChanges();

            var allCast = await repository.GetAll();

            // Assert
            allCast.Should().NotBeNullOrEmpty();
            allCast.Should().HaveCount(4);
            allCast.Should().AllSatisfy(cast =>
            {
                cast.Should().NotBeNull();
                cast.TitleId.Should().BeOneOf([1297469, 1489427, 1519096, 1522910]);
                cast.Person.Should().NotBeNull();
                cast.Person.Id.Should().BeOneOf([1, 2, 3, 4]);
                cast.PersonId.Should().BeOneOf([1, 2, 3, 4]);
                cast.Role.Should().Be(PersonRole.Actor);
            });
        }

        [Fact]
        public async Task ShouldSaveOnlyOneCast()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            context.Titles.Add(new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161")
            {
                Id = 1297469,
                Relevance = 11.13m
            });
            context.People.Add(new Person("Tim Robbins"));

            var repository = new CastRepository(context);
            Cast cast = new Cast
            {
                Id = 1,
                TitleId = 1297469,
                PersonId = 1,
                Role = PersonRole.Actor
            };

            // Act
            await repository.RegisterAsync(cast);
            await repository.SaveChanges();

            var allCast = await repository.GetAll();
            var castById = await repository.GetByIdAsync(1);

            // Assert
            allCast.Should().NotBeNullOrEmpty();
            allCast.Should().HaveCount(1);
            allCast.First().Id.Should().Be(1);
            castById.Should().NotBeNull();
            castById.Id.Should().Be(1);
            castById.PersonId.Should().Be(1);
            castById.Person.Id.Should().Be(1);
            castById.Person.Name.Should().Be("Tim Robbins");
            castById.Title.Id.Should().Be(1297469);
            castById.Title.Name.Should().Be("The Shawshank Redemption");
            castById.Title.Type.Should().Be(TitleType.Movie);
            castById.Title.ReleaseDate.Should().Be(1994);
            castById.Title.Relevance.Should().Be(11.13m);
            castById.Title.IMDB_Id.Should().Be("tt0111161");
            castById.TitleId.Should().Be(1297469);
            castById.Role.Should().Be(PersonRole.Actor);
        }
    }
}
