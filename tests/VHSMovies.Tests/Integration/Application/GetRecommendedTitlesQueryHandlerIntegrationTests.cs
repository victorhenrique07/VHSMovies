using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Handlers;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Application
{
    [Collection("DatabaseCollection")]
    public class GetRecommendedTitlesQueryHandlerIntegrationTests
    {
        private readonly DatabaseSetupFixture _fixture;

        public GetRecommendedTitlesQueryHandlerIntegrationTests(DatabaseSetupFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_Should_ReturnExpectedResults_WithIncludeGenres()
        {
            // Arrange
            var handler = new GetRecommendedTitlesQueryHandler(
                new RecommendedTitlesRepository(_fixture.Context),
                new CastRepository(_fixture.Context),
                new GenreRepository(_fixture.Context)
            );

            var query = new GetRecommendedTitlesQuery
            {
                IncludeGenres = new HashSet<int> { 1 },
                TitlesAmount = 3
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().Contain(t => t.Genres.Any(g => g.Name == "Action"));
        }

        [Fact]
        public async Task Handle_Should_ReturnExpectedResults_WithExcludeGenres()
        {
            // Arrange
            var handler = new GetRecommendedTitlesQueryHandler(
                new RecommendedTitlesRepository(_fixture.Context),
                new CastRepository(_fixture.Context),
                new GenreRepository(_fixture.Context)
            );

            var query = new GetRecommendedTitlesQuery
            {
                ExcludeGenres = new HashSet<int> { 9 },
                TitlesAmount = 5
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().Contain(t => t.Genres.Any(g => g.Name != "Drama"));
        }
    }
}
