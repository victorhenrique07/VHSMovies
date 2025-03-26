using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Handlers;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Tests.Setup;

namespace VHSMovies.Tests.Application
{
    public class GetMostRelevantTitlesQueryHandlerTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture fixture;

        public GetMostRelevantTitlesQueryHandlerTests(BaseTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(new int[] { 1, 15 })]
        [InlineData(new int[] { 15, 9 })]
        [InlineData(new int[] { 7, 15 })]
        [InlineData(new int[] { 7, 17 })]
        [InlineData(new int[] { 14, 4 })]
        [InlineData(new int[] { 7, 14 })]
        public async Task Handle_Should_Return12ActionTitles(int[] genreIds)
        {
            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            // Arrange
            var query = new GetMostRelevantTitlesQuery()
            {
                TitlesAmount = 12,
                GenresId = genreIds
            };

            fixture.recommendedTitlesMock
                .Setup(r => r.Query())
                .Returns(fixture.AllTitles.AsQueryable());

            var handler = new GetMostRelevantTitlesQueryHandler(fixture.recommendedTitlesMock.Object, fixture.genresRepositoryMock.Object);

            // Act
            IReadOnlyCollection<TitleResponse> response = await handler.Handle(query, default);

            // Assert
            response.Should().NotBeEmpty();
            response.Should().HaveCount(12);
            response.Should().AllSatisfy(x =>
            {
                x.Genres.Any(g => genreIds.Contains(g.Id)).Should().BeTrue();
            });
        }

    }
}
