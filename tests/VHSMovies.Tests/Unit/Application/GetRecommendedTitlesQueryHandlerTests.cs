using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Castle.Components.DictionaryAdapter.Xml;

using FluentAssertions;

using Moq;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Handlers;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;

using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace VHSMovies.Tests.Unit.Application
{
    public class GetRecommendedTitlesQueryHandlerTests
    {
        private readonly Mock<IRecommendedTitlesRepository> _recommendedRepoMock = new();
        private readonly Mock<IGenreRepository> _genreRepoMock = new();
        private readonly Mock<ICastRepository> _castRepoMock = new();

        private readonly GetRecommendedTitlesQueryHandler _handler;

        public GetRecommendedTitlesQueryHandlerTests()
        {
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Drama" },
                new Genre { Id = 3, Name = "Horror" },
                new Genre { Id = 4, Name = "Adventure" },
                new Genre { Id = 5, Name = "Animation" },
                new Genre { Id = 6, Name = "Comedy" },
                new Genre { Id = 7, Name = "Crime" },
                new Genre { Id = 8, Name = "Documentary" },
                new Genre { Id = 9, Name = "Family" },
                new Genre { Id = 10, Name = "Fantasy" },
                new Genre { Id = 11, Name = "History" },
                new Genre { Id = 12, Name = "Music" },
                new Genre { Id = 13, Name = "Mystery" },
                new Genre { Id = 14, Name = "Romance" },
                new Genre { Id = 15, Name = "Science Fiction" },
                new Genre { Id = 16, Name = "TV Movie" },
                new Genre { Id = 17, Name = "Thriller" },
                new Genre { Id = 18, Name = "War" },
                new Genre { Id = 19, Name = "Western" }
            };

            _genreRepoMock.Setup(g => g.GetAll()).ReturnsAsync(genres);

            _handler = new GetRecommendedTitlesQueryHandler(
                _recommendedRepoMock.Object,
                _castRepoMock.Object,
                _genreRepoMock.Object
            );

            darkKnightRecommendedTitle = new RecommendedTitle
            {
                Id = darkKnightTitle.Id,
                Name = darkKnightTitle.Name,
                AverageRating = darkKnightTitle.AverageRating,
                ReleaseDate = darkKnightTitle.ReleaseDate,
                Relevance = darkKnightTitle.Relevance,
                Genres = new[] { "Drama", "Action" }
            };

            sharshankRecommendedTitle = new RecommendedTitle
            {
                Id = shawshankTitle.Id,
                Name = shawshankTitle.Name,
                AverageRating = shawshankTitle.AverageRating,
                ReleaseDate = shawshankTitle.ReleaseDate,
                Relevance = shawshankTitle.Relevance,
                Genres = new[] { "Drama" }
            };

            inceptionRecommendedTitle = new RecommendedTitle
            {
                Id = inceptionTitle.Id,
                Name = inceptionTitle.Name,
                AverageRating = inceptionTitle.AverageRating,
                ReleaseDate = inceptionTitle.ReleaseDate,
                Relevance = inceptionTitle.Relevance,
                Genres = new[] { "Action", "Drama" }
            };

            titles = new List<RecommendedTitle>
            {
                darkKnightRecommendedTitle,
                sharshankRecommendedTitle,
                inceptionRecommendedTitle
            };
        }

        private readonly Title darkKnightTitle = new Title("The Dark Knight", TitleType.Movie, 2008, "tt0468569")
        {
            Id = 1489427,
            Relevance = 10.98m,
            Genres = new List<TitleGenre>()
            {
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 1,
                        Name = "Action"
                    }
                },
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 9,
                        Name = "Drama"
                    }
                },
            },
            Ratings = new List<Review>
            {
                new Review("IMDb", 9.0m, 2600000)
                {
                    Id = 3,
                    TitleExternalId = "tt0468569"
                },
                new Review("Metacritic", 84.0m, 85000)
                {
                    Id = 4,
                    TitleExternalId = "mt0468569"
                }
            }
        };

        private readonly Title shawshankTitle = new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161")
        {
            Id = 1297469,
            Relevance = 11.13m,
            Genres = new List<TitleGenre>()
            {
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 9,
                        Name = "Drama"
                    }
                }
            },
            Ratings = new List<Review>
            {
                new Review("IMDb", 9.3m, 2700000)
                {
                    Id = 1,
                    TitleExternalId = "tt0111161"
                },
                new Review("Metacritic", 80.0m, 80_000)
                {
                    Id = 2,
                    TitleExternalId = "mt0111161"
                },
            }
        };

        private readonly Title inceptionTitle = new Title("Inception", TitleType.Movie, 2010, "tt1375666")
        {
            Id = 1234567,
            Relevance = 9.5m,
            Genres = new List<TitleGenre>()
            {
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 1,
                        Name = "Action"
                    }
                },
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 9,
                        Name = "Drama"
                    }
                },
            },
            Ratings = new List<Review>()
            {
                new Review("IMDb", 9.2m, 2000000)
                {
                    Id = 5,
                    TitleExternalId = "tt1375666"
                },
                new Review("Metacritic", 100.0m, 75000)
                {
                    Id = 6,
                    TitleExternalId = "tt1375666"
                },
            }
        };

        private readonly RecommendedTitle darkKnightRecommendedTitle;

        private readonly RecommendedTitle sharshankRecommendedTitle;

        private readonly RecommendedTitle inceptionRecommendedTitle;

        private readonly IReadOnlyCollection<RecommendedTitle> titles;

        [Fact]
        public async Task ShouldReturnsAllTitlesWhenNoFilterIsApplied()
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
        }

        [Theory]
        [InlineData(new int[] { 1489427, 1297469 }, 1234567)]
        [InlineData(new int[] { 1234567, 1297469 }, 1489427)]
        [InlineData(new int[] { 1234567, 1489427 }, 1297469)]
        public async Task ShouldExcludesTitlesWhenTitlesToExcludeIsProvided(int[] excludedTitles, int expectedTitleId)
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesToExclude = excludedTitles
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(t => t.Id == expectedTitleId);
            result.Should().NotContain(result => excludedTitles.Contains(result.Id));
        }

        [Fact]
        public async Task ShouldReturnTitlesAverageRatingGreaterOrEqualThan4()
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                MinimumRating = 4
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3);
            result.Should().NotContain(t => t.AverageRating < 4);
        }

        [Fact]
        public async Task ShouldReturnInceptionTitleWhenYearsRangeStartsAt2009()
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                YearsRange = new[] { 2009, 2020 }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            DateOnly value = result.First().ReleaseDate.Value;

            result.Should().ContainSingle();
            result.First().Id.Should().Be(inceptionTitle.Id);
            result.First().Name.Should().Be(inceptionTitle.Name);
            result.First().AverageRating.Should().Be(inceptionTitle.AverageRating);
            value.Year.Should().Be(inceptionTitle.ReleaseDate);
            result.First().Genres.Should().AllSatisfy(genre =>
            {
                genre.Name.Should().BeOneOf(inceptionRecommendedTitle.Genres);
            });
        }

        [Fact]
        public async Task ShouldReturnEmptyListWhenNoTitleMatchesActor()
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var fixture = new Fixture();

            fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customize<Cast>(composer =>
                composer.With(x => x.Role, PersonRole.Actor));

            IReadOnlyCollection<Cast> cast = fixture.CreateMany<Cast>(10).ToList();

            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Actor)).ReturnsAsync(cast);

            var query = new GetRecommendedTitlesQuery
            {
                Actors = new List<string> { "Nonexistent Actor" }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnShawshankMovieWithExcludeGenresEqualToAction()
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                IncludeGenres = new HashSet<int> { 2 },
                ExcludeGenres = new HashSet<int> { 1 },
                MustInclude = new HashSet<int> { 2 }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainSingle();
            result.Should().ContainSingle(t => t.Id == 1297469);
            result.First().Genres.Should().Contain(g => g.Id == 2);
            result.First().Genres.Should().Contain(g => g.Name == "Drama");
            result.First().Name.Should().Be("The Shawshank Redemption");
        }

        [Fact]
        public async Task ShouldReturnDarkKnightWhenFilterByActorA()
        {
            // Arrange
            List<Cast> cast = new List<Cast>
            {
                new() { Title = darkKnightTitle, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { Title = inceptionTitle, Person = new Person("Actor B"), Role = PersonRole.Actor }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Actor)).ReturnsAsync(cast);

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery
            {
                Actors = new List<string> { "Actor A" }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainSingle();
            result.Should().Contain(t => t.Id == 1489427);
            result.Should().NotContain(t => t.Id == 1234567 && t.Id == 1297469);

        }

        [Fact]
        public async Task ShouldReturnShawshankWhenFilterByDirectorA()
        {
            // Arrange
            var cast = new List<Cast>
            {
                new() { Title = shawshankTitle, Person = new Person("Director A"), Role = PersonRole.Director },
                new() { Title = inceptionTitle, Person = new Person("Director B"), Role = PersonRole.Director }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Director)).ReturnsAsync(cast);

            var query = new GetRecommendedTitlesQuery
            {
                Directors = new List<string> { "Director A" }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainSingle();
            result.Should().Contain(t => t.Id == 1297469);
            result.Should().NotContain(t => t.Id == 1234567 && t.Id == 1489427);
        }

        [Fact]
        public async Task ShouldReturnShawshankWhenFilterByWriterA()
        {
            // Arrange
            var cast = new List<Cast>
            {
                new() { Title = shawshankTitle, Person = new Person("Writer A"), Role = PersonRole.Writer },
                new() { Title = darkKnightTitle, Person = new Person("Writer B"), Role = PersonRole.Writer }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Writer)).ReturnsAsync(cast);

            var query = new GetRecommendedTitlesQuery
            {
                Writers = new List<string> { "Writer A" }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().ContainSingle();
            result.Should().Contain(t => t.Id == 1297469);
            result.Should().NotContain(t => t.Id == 1234567 && t.Id == 1489427);
        }

        [Theory]
        [InlineData(new[] { "Actor A" }, new string[0], new string[0], new[] { 1297469, 1489427 })]
        [InlineData(new string[0], new[] { "Director A" }, new string[0], new[] { 1234567, 1489427 })]
        [InlineData(new string[0], new string[0], new[] { "Writer A" }, new[] { 1234567, 1489427 })]
        [InlineData(new[] { "Actor A" }, new[] { "Director A" }, new string[0], new[] { 1489427 })]
        [InlineData(new[] { "Actor A" }, new[] { "Director A" }, new[] { "Writer A" }, new[] { 1489427 })]
        [InlineData(new[] { "Actor A" }, new[] { "Director A" }, new[] { "Writer C" }, new int[0])]
        [InlineData(new string[0], new string[0], new string[0], new[] { 1234567, 1297469, 1489427 })]
        public async Task ShouldReturnCorrectTitlesWithPersonFilters(
            string[] actors, string[] directors, string[] writers, int[] expectedIds)
        {
            // Arrange
            var cast = new List<Cast>
            {
                new() { Title = darkKnightTitle, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { Title = darkKnightTitle, Person = new Person("Actor B"), Role = PersonRole.Actor },
                new() { Title = darkKnightTitle, Person = new Person("Director A"), Role = PersonRole.Director },
                new() { Title = darkKnightTitle, Person = new Person("Writer A"), Role = PersonRole.Writer },
                new() { Title = shawshankTitle, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { Title = shawshankTitle, Person = new Person("Writer B"), Role = PersonRole.Writer },
                new() { Title = inceptionTitle, Person = new Person("Director A"), Role = PersonRole.Director },
                new() { Title = inceptionTitle, Person = new Person("Writer A"), Role = PersonRole.Writer },
                new() { Title = inceptionTitle, Person = new Person("Actor C"), Role = PersonRole.Actor },
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(It.IsAny<PersonRole>())).ReturnsAsync((PersonRole role) =>
                cast.Where(x => x.Role == role).ToList());

            var query = new GetRecommendedTitlesQuery
            {
                Actors = actors.ToList(),
                Directors = directors.ToList(),
                Writers = writers.ToList()
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Select(r => r.Id).Should().BeEquivalentTo(expectedIds);
        }

        [Fact]
        public async Task ShouldReturnEmptyMoviesListWhenYearOutOfRange()
        {
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                YearsRange = new[] { 2015, 2020 }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnMoviesOrderedByRelevanceAndLimitsAmount()
        {
            // Arrange
            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 2
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1297469);
            result.Last().Id.Should().Be(1489427);
        }

        [Fact]
        public async Task Handle_AllFiltersCombined_ReturnsExpectedResult()
        {
            IQueryable<RecommendedTitle> titles = new List<RecommendedTitle>
            {
                darkKnightRecommendedTitle,
                sharshankRecommendedTitle,
                inceptionRecommendedTitle
            }.AsQueryable();

            var cast = new List<Cast>
            {
                new() { Title = shawshankTitle, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { Title = shawshankTitle, Person = new Person("Director A"), Role = PersonRole.Director },
                new() { Title = shawshankTitle, Person = new Person("Writer A"), Role = PersonRole.Writer }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(It.IsAny<PersonRole>())).ReturnsAsync((PersonRole role) =>
                cast.Where(x => x.Role == role).ToList());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 5,
                MinimumRating = 4.0m,
                YearsRange = new[] { 1992, 2000 },
                Actors = new List<string> { "Actor A" },
                Directors = new List<string> { "Director A" },
                Writers = new List<string> { "Writer A" },
                IncludeGenres = new HashSet<int> { 2 },
                ExcludeGenres = new HashSet<int> { 1 },
                MustInclude = new HashSet<int> { 2 }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(1297469);
            result.First().Genres.Should().Contain(result => result.Id == 2);
            result.First().Genres.Should().Contain(result => result.Name == "Drama");
            result.First().Name.Should().Be("The Shawshank Redemption");
            result.First().AverageRating.Should().BeGreaterThan(4.0m);
        }
    }
}
