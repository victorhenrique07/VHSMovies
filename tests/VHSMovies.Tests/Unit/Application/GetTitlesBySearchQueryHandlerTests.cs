using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Moq;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Handlers;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infrastructure.Services;

namespace VHSMovies.Tests.Unit.Application
{
    public class GetTitlesBySearchQueryHandlerTests
    {
        private readonly Mock<IRecommendedTitlesRepository> _mockRepository;
        private readonly Mock<ITMDbService> _tmdbService;
        private readonly GetTitlesBySearchQueryHandler _handler;

        public GetTitlesBySearchQueryHandlerTests()
        {
            _mockRepository = new Mock<IRecommendedTitlesRepository>();
            _tmdbService = new Mock<ITMDbService>();
            _handler = new GetTitlesBySearchQueryHandler(_mockRepository.Object, _tmdbService.Object);
        }

        /*[Theory]
        [InlineData("The", 3)]
        public async Task Handle_ReturnsFilteredTitles_WhenSearchQueryMatches(string searchQuery, int expectedTitlesCount)
        {
            // Arrange
            var query = new GetTitlesBySearchQuery()
            {
                SearchQuery = searchQuery
            };

            var fakeTitles = new List<RecommendedTitle>
            {
                new RecommendedTitle { Name = "The Shawshank Redemption" },
                new RecommendedTitle { Name = "The Dark Knight" },
                new RecommendedTitle { Name = "The Godfather" },
                new RecommendedTitle { Name = "Game of Thrones" },
                new RecommendedTitle { Name = "Band of Brothers" }
            }.AsQueryable();

            // Act
            var filteredTitles = fakeTitles
                .Where(t => t.Name.ToLower().Contains(searchQuery.ToLower()))
                .AsQueryable();

            _mockRepository.Setup(repo => repo.Query()).Returns(filteredTitles);

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(expectedTitlesCount);
            result.Should().AllSatisfy(title =>
            {
                title.Name.Should().Contain(searchQuery);
                title.Name.Should().NotContain("Game of Thrones");
                title.Name.Should().NotContain("Band of Brothers");
            });
        }*/

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoMatchFound()
        {
            // Arrange
            var searchQuery = "Breaking";

            var query = new GetTitlesBySearchQuery()
            {
                SearchQuery = searchQuery
            };

            var fakeTitles = new List<RecommendedTitle>
            {
                new RecommendedTitle { Name = "The Shawshank Redemption" },
                new RecommendedTitle { Name = "The Dark Knight" },
                new RecommendedTitle { Name = "The Godfather" },
                new RecommendedTitle { Name = "Game of Thrones" },
                new RecommendedTitle { Name = "Band of Brothers" }
            }.AsQueryable();

            // Act
            var filteredTitles = fakeTitles
                .Where(t => t.Name.ToLower().Contains(searchQuery.ToLower()))
                .AsQueryable();

            _mockRepository.Setup(repo => repo.Query()).Returns(filteredTitles);

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNullOrEmpty();
            result.Should().HaveCount(0);
        }
    }
}
