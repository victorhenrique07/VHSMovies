using AutoFixture;
using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Handlers;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.Tests.Unit.Application
{
    public class GetRecommendedTitlesQueryHandlerTests
    {
        private readonly Mock<IRecommendedTitlesRepository> _recommendedRepoMock = new();
        private readonly Mock<IGenreRepository> _genreRepoMock = new();
        private readonly Mock<ICastRepository> _castRepoMock = new();
        private readonly Mock<HttpClient> _httpClientMock = new();

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
        }

        [Fact]
        public async Task Handle_ReturnsAllTitles_WhenNoFilterIsApplied()
        {
            var fixture = new Fixture();
            var random = new Random();

            fixture.Customize<DateOnly>(c => c.FromFactory(() => new DateOnly(2023, 1, 1)));

            fixture.Customize<RecommendedTitle>(composer =>
                composer.With(x => x.Genres, new[] {random.Next(0, 2) == 0 ? "Action" : "Drama" }));

            var titles = fixture.CreateMany<RecommendedTitle>(10).AsQueryable();

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);

            var query = new GetRecommendedTitlesQuery { TitlesAmount = 10 };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task Handle_ExcludesTitles_WhenTitlesToExcludeIsProvided()
        {
            var fixture = new Fixture();

            fixture.Customize<DateOnly>(c => c.FromFactory(() => new DateOnly(2023, 1, 1)));

            var titles = fixture.CreateMany<RecommendedTitle>(10).Select((title, index) =>
            {
                title.Id = index + 1;
                title.Genres = Array.Empty<string>();
                return title;
            }).ToList();

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles.AsQueryable());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                TitlesToExclude = new List<int> { 2 }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(9);
            result.Should().NotContain(t => t.Id == 2);
        }

        [Fact]
        public async Task Handle_FiltersByMinimumRating()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, AverageRating = 4.5m, Relevance = 1, Genres = Array.Empty<string>() },
                new() { Id = 2, AverageRating = 3.0m, Relevance = 2, Genres = Array.Empty<string>() },
            }.AsQueryable();

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                MinimumRating = 4
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task Handle_FiltersByYearsRange()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, ReleaseDate = 2010, Relevance = 1, Genres = Array.Empty<string>() },
                new() { Id = 2, ReleaseDate = 2022, Relevance = 2, Genres = Array.Empty<string>() },
            }.AsQueryable();

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                YearsRange = new[] { 2009, 2020 }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ReturnsEmpty_WhenNoTitleMatchesActor()
        {
            _recommendedRepoMock.Setup(r => r.Query()).Returns(new List<RecommendedTitle>
            {
                new() { Id = 1, Relevance = 1, Genres = Array.Empty<string>() },
            }.AsQueryable());

            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Actor)).ReturnsAsync(new List<Cast>());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                Actors = new List<string> { "Nonexistent Actor" }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WithGenreFilters_FiltersTitlesCorrectly()
        {
            var allTitles = new List<RecommendedTitle>
            {
                new RecommendedTitle { Id = 1, Genres = new [] { "Action", "Drama" }, Relevance = 0.9m },
                new RecommendedTitle { Id = 2, Genres = new [] { "Action", "Horror" }, Relevance = 0.8m },
                new RecommendedTitle { Id = 3, Genres = new [] { "Drama" }, Relevance = 0.7m },
                new RecommendedTitle { Id = 4, Genres = new [] { "Action" }, Relevance = 0.95m },
            };

            var query = new GetRecommendedTitlesQuery
            {
                IncludeGenres = new HashSet<int> { 1, 2 },
                ExcludeGenres = new HashSet<int> { 3 },
                MustInclude = new HashSet<int> { 1 },
                TitlesAmount = 10
            };

            _recommendedRepoMock.Setup(repo => repo.Query()).Returns(allTitles.AsQueryable());

            var result = await _handler.Handle(query, CancellationToken.None);

            var resultIds = result.Select(r => r.Id).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(resultIds, id => id == 1);
            Assert.Contains(resultIds, id => id == 4);
            Assert.DoesNotContain(resultIds, id => id == 2);
            Assert.DoesNotContain(resultIds, id => id == 3);
        }

        [Fact]
        public async Task Handle_WithOnlyActors_FiltersCorrectly()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, Relevance = 3.5m, Genres = Array.Empty<string>() },
                new() { Id = 2, Relevance = 2.1m, Genres = Array.Empty<string>()},
                new() { Id = 3, Relevance = 4.8m, Genres = Array.Empty<string>()}
            }.AsQueryable();

            var cast = new List<Cast>
            {
                new() { TitleId = 1, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { TitleId = 3, Person = new Person("Actor B"), Role = PersonRole.Actor }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Actor)).ReturnsAsync(cast);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                Actors = new List<string> { "Actor A" }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1);
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task Handle_WithOnlyDirectors_FiltersCorrectly()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, Relevance = 1.5m, Genres = Array.Empty<string>() },
                new() { Id = 2, Relevance = 3.5m, Genres = Array.Empty<string>() }
            }.AsQueryable();

            var cast = new List<Cast>
            {
                new() { TitleId = 2, Person = new Person("Director A"), Role = PersonRole.Director }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Director)).ReturnsAsync(cast);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                Directors = new List<string> { "Director A" }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(2);
        }

        [Fact]
        public async Task Handle_WithOnlyWriters_FiltersCorrectly()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, Relevance = 1.2m, Genres = Array.Empty<string>() },
                new() { Id = 2, Relevance = 2.2m, Genres = Array.Empty<string>() }
            }.AsQueryable();

            var cast = new List<Cast>
            {
                new() { TitleId = 1, Person = new Person("Writer A"), Role = PersonRole.Writer }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(PersonRole.Writer)).ReturnsAsync(cast);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                Writers = new List<string> { "Writer A" }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task Handle_WithAllPeopleFilters_IntersectsResultsCorrectly()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, Relevance = 4.0m, Genres = Array.Empty<string>() },
                new() { Id = 2, Relevance = 3.0m, Genres = Array.Empty<string>()},
                new() { Id = 3, Relevance = 5.0m, Genres = Array.Empty<string>()}
            }.AsQueryable();

            var cast = new List<Cast>
            {
                new() { TitleId = 1, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { TitleId = 1, Person = new Person("Director A"), Role = PersonRole.Director },
                new() { TitleId = 1, Person = new Person("Writer A"), Role = PersonRole.Writer },
                new() { TitleId = 2, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { TitleId = 3, Person = new Person("Director A"), Role = PersonRole.Director }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(It.IsAny<PersonRole>())).ReturnsAsync((PersonRole role) =>
                cast.Where(x => x.Role == role).ToList());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                Actors = new List<string> { "Actor A" },
                Directors = new List<string> { "Director A" },
                Writers = new List<string> { "Writer A" }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task Handle_YearsRangeOutsideAllTitles_ReturnsEmpty()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, ReleaseDate = 2000, Relevance = 2.0m },
                new() { Id = 2, ReleaseDate = 2005, Relevance = 2.5m }
            }.AsQueryable();

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 10,
                YearsRange = new[] { 2015, 2020 }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_OrdersByRelevanceAndLimitsAmount()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, Relevance = 2.0m, Genres = Array.Empty<string>() },
                new() { Id = 2, Relevance = 5.0m, Genres = Array.Empty<string>() },
                new() { Id = 3, Relevance = 3.0m, Genres = Array.Empty < string >() }
            }.AsQueryable();

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 2
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(2);
            result.First().Id.Should().Be(2);
            result.Last().Id.Should().Be(3);
        }

        [Fact]
        public async Task Handle_AllFiltersCombined_ReturnsExpectedResult()
        {
            var titles = new List<RecommendedTitle>
            {
                new() { Id = 1, AverageRating = 4.5m, ReleaseDate = 2020, Relevance = 4.5m, Genres = new[] { "Drama", "Action" } },
                new() { Id = 2, AverageRating = 3.5m, ReleaseDate = 2018, Relevance = 3.0m, Genres = new[] { "Horror" } }
            }.AsQueryable();

            var cast = new List<Cast>
            {
                new() { TitleId = 1, Person = new Person("Actor A"), Role = PersonRole.Actor },
                new() { TitleId = 1, Person = new Person("Director A"), Role = PersonRole.Director },
                new() { TitleId = 1, Person = new Person("Writer A"), Role = PersonRole.Writer }
            };

            _recommendedRepoMock.Setup(r => r.Query()).Returns(titles);
            _castRepoMock.Setup(c => c.GetCastsByPersonRole(It.IsAny<PersonRole>())).ReturnsAsync((PersonRole role) =>
                cast.Where(x => x.Role == role).ToList());

            var query = new GetRecommendedTitlesQuery
            {
                TitlesAmount = 5,
                MinimumRating = 4.0m,
                YearsRange = new[] { 2019, 2021 },
                Actors = new List<string> { "Actor A" },
                Directors = new List<string> { "Director A" },
                Writers = new List<string> { "Writer A" },
                IncludeGenres = new HashSet<int> { 1, 2 },
                ExcludeGenres = new HashSet<int> { 3 },
                MustInclude = new HashSet<int> { 1 }
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().ContainSingle();
            result.First().Id.Should().Be(1);
        }
    }
}
