using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Handlers;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.Test
{
    public class RecommenddedTitlesTest
    {
        private readonly Mock<ITitleRepository<Title>> titleRepositoryMock;
        private readonly Mock<ITitleGenreRepository> titleGenreRepositoryMock;
        private readonly Mock<ICastRepository> castRepositoryMock;

        public RecommenddedTitlesTest()
        {
            this.titleRepositoryMock = new();
            this.titleGenreRepositoryMock = new();
            this.castRepositoryMock = new();

            var fixture = new Fixture();

            fixture.Customize<TitleResponse>(c => c.With(t => t.Genres, new List<GenreResponse>
            {
                new GenreResponse { Id = 28, Name = "Action" },
                new GenreResponse { Id = 878, Name = "SciFi" },
                new GenreResponse { Id = 14, Name = "Fantasy" }
            }));

            this.Titles_Action_SciFi_Fantasy = fixture.CreateMany<TitleResponse>(5).ToList();

            fixture.Customize<TitleResponse>(c => c.With(t => t.Genres, new List<GenreResponse>
            {
                new GenreResponse { Id = 18, Name = "Drama" },
                new GenreResponse { Id = 878, Name = "SciFi" },
            }));

            this.Titles_Drama_SciFi = fixture.CreateMany<TitleResponse>(5).ToList();

            fixture.Customize<TitleResponse>(c => c.With(t => t.Genres, new List<GenreResponse>
            {
                new GenreResponse { Id = 10749, Name = "Romance" },
                new GenreResponse { Id = 35, Name = "Comedy" },
            }));

            this.Titles_Romance_Comedy = fixture.CreateMany<TitleResponse>(5).ToList();
        }

        private readonly IReadOnlyCollection<TitleResponse> Titles_Action_SciFi_Fantasy;
        private readonly IReadOnlyCollection<TitleResponse> Titles_Drama_SciFi;
        private readonly IReadOnlyCollection<TitleResponse> Titles_Romance_Comedy;

        [Fact]
        public async Task Test_If_RecommendedTitlesQueryHandler_Is_Returning_Action_SciFi_Fantasy_Titles()
        {
            // Arrange
            int[] genresId = [28, 878, 14];

            List<Title> titles = new List<Title>();
            List<TitleGenre> titlesGenres = new List<TitleGenre>();

            foreach (TitleResponse title in Titles_Action_SciFi_Fantasy)
            {
                Title titleToAdd = new Title(title.Name, title.Description, new List<Review>())
                {
                    Id = title.Id
                };

                titles.Add(titleToAdd);

                Genre action = new Genre()
                {
                    Id = 28,
                    Name = "Action"
                };
                Genre sciFi = new Genre()
                {
                    Id = 878,
                    Name = "SciFi"
                };
                Genre fantasy = new Genre()
                {
                    Id = 14,
                    Name = "Fantasy"
                };

                titlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = action, TitleId = title.Id, GenreId = action.Id});
                titlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = sciFi, TitleId = title.Id, GenreId = sciFi.Id });
                titlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = fantasy, TitleId = title.Id, GenreId = fantasy.Id });
            }

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                Genres = genresId
            };

            titleGenreRepositoryMock
                .Setup(g => g.GetTitlesByGenreId(genresId))
                .ReturnsAsync(titlesGenres);

            titleRepositoryMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(titles);

            var handler = new GetRecommendedTitlesQueryHandler(titleRepositoryMock.Object, castRepositoryMock.Object, titleGenreRepositoryMock.Object);

            // Act
            IReadOnlyCollection<TitleResponse> response = await handler.Handle(query, CancellationToken.None);

            response.Should().NotBeEmpty();
            response.Should().HaveCount(5);
        }
    }
}
