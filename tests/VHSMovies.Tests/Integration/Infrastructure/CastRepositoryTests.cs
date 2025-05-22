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
                cast.Title.Id.Should().Be(titleId);
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

            var titles = _fixture.GetTitlesList();
            var people = _fixture.GetPersonList();

            context.Titles.AddRange(titles);
            context.People.AddRange(people);
            context.SaveChanges();

            var castRepository = new CastRepository(context);
            var titleRepository = new TitleRepository(context);
            var personRepository = new PersonRepository(context);

            Title shawshank = await titleRepository.GetByIdAsync(1297469);
            Title darkKnight = await titleRepository.GetByIdAsync(1489427);
            Title godfather = await titleRepository.GetByIdAsync(1519096);
            Title breakingBad = await titleRepository.GetByIdAsync(1522910);
            Person timRobbins = await personRepository.GetPersonById(1);
            Person christianBale = await personRepository.GetPersonById(2);
            Person bryanCranston = await personRepository.GetPersonById(3);
            Person emiliaClarke = await personRepository.GetPersonById(4);

            IReadOnlyCollection<Cast> casts = new List<Cast>()
            {
                new Cast
                {
                    Id = 1,
                    Title = shawshank,
                    Person = timRobbins,
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 4,
                    Title = darkKnight,
                    Person = christianBale,
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 7,
                    Title = godfather,
                    Person = bryanCranston,
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 9,
                    Title = breakingBad,
                    Person = emiliaClarke,
                    Role = PersonRole.Actor
                },
            };

            // Act
            await castRepository.RegisterListAsync(casts);
            await castRepository.SaveChanges();

            var allCast = await castRepository.GetAll();

            // Assert
            allCast.Should().NotBeNullOrEmpty();
            allCast.Should().HaveCount(4);
            allCast.Should().AllSatisfy(cast =>
            {
                cast.Should().NotBeNull();
                cast.Title.Id.Should().BeOneOf([1297469, 1489427, 1519096, 1522910]);
                cast.Person.Should().NotBeNull();
                cast.Person.Id.Should().BeOneOf([1, 2, 3, 4]);
                cast.Role.Should().Be(PersonRole.Actor);
            });
        }

        [Fact]
        public async Task ShouldSaveOnlyOneCast()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            context.Titles.Add(
                new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161")
                {
                    Id = 1297469,
                    Relevance = 11.13m
                }
            );

            context.People.Add(new Person("Tim Robbins"));
            context.SaveChanges();

            var castRepository = new CastRepository(context);
            var titleRepository = new TitleRepository(context);
            var personRepository = new PersonRepository(context);

            Title shawshank = await titleRepository.GetByIdAsync(1297469);
            Person timRobbins = await personRepository.GetPersonById(1);

            Cast cast = new Cast
            {
                Id = 1,
                Title = shawshank,
                Person = timRobbins,
                Role = PersonRole.Actor
            };

            // Act
            await castRepository.RegisterAsync(cast);
            await castRepository.SaveChanges();

            var allCast = await castRepository.GetAll();
            var castById = await castRepository.GetByIdAsync(1);

            // Assert
            allCast.Should().NotBeNullOrEmpty();
            allCast.Should().HaveCount(1);
            allCast.First().Id.Should().Be(1);
            castById.Should().NotBeNull();
            castById.Id.Should().Be(1);
            castById.Person.Id.Should().Be(1);
            castById.Person.Name.Should().Be("Tim Robbins");
            castById.Title.Id.Should().Be(1297469);
            castById.Title.Name.Should().Be("The Shawshank Redemption");
            castById.Title.Type.Should().Be(TitleType.Movie);
            castById.Title.ReleaseDate.Should().Be(1994);
            castById.Title.Relevance.Should().Be(11.13m);
            castById.Title.IMDB_Id.Should().Be("tt0111161");
            castById.Role.Should().Be(PersonRole.Actor);
        }
    }
}
