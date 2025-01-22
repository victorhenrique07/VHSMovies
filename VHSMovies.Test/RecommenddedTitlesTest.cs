using AutoFixture;
using FakeItEasy;
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
                new GenreResponse(28, "Action"),
                new GenreResponse(878, "SciFi"),
                new GenreResponse(14, "Fantasy")
            }));

            this.Titles_Action_SciFi_Fantasy = fixture.CreateMany<TitleResponse>(5).ToList();

            fixture.Customize<TitleResponse>(c => c.With(t => t.Genres, new List<GenreResponse>
            {
                new GenreResponse(18, "Drama"),
                new GenreResponse(878, "SciFi")
            }));

            this.Titles_Drama_SciFi = fixture.CreateMany<TitleResponse>(5).ToList();

            fixture.Customize<TitleResponse>(c => c.With(t => t.Genres, new List<GenreResponse>
            {
                new GenreResponse(10749, "Romance"),
                new GenreResponse(35, "Comedy")
            }));

            this.Titles_Romance_Comedy = fixture.CreateMany<TitleResponse>(5).ToList();

            PopulateAllTitles();
            PopulateAllTitleGenres();
        }

        private readonly IReadOnlyCollection<TitleResponse> Titles_Action_SciFi_Fantasy;
        private readonly IReadOnlyCollection<TitleResponse> Titles_Drama_SciFi;
        private readonly IReadOnlyCollection<TitleResponse> Titles_Romance_Comedy;

        private readonly IList<Title> AllTitles = new List<Title>();
        private readonly IList<TitleGenre> AllTitlesGenres = new List<TitleGenre>();

        [Fact]
        public async Task Test_If_RecommendedTitlesQueryHandler_Is_Returning_Action_SciFi_Fantasy_Titles()
        {
            // Arrange
            int[] genresId = [28, 878, 14];

            List<TitleGenre> titlesGenres = new List<TitleGenre>();

            foreach (TitleResponse title in Titles_Action_SciFi_Fantasy)
            {
                Title titleToAdd = new Title(title.Name, title.Description, new List<Review>())
                {
                    Id = title.Id
                };

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
                .ReturnsAsync(AllTitles);

            var handler = new GetRecommendedTitlesQueryHandler(titleRepositoryMock.Object, castRepositoryMock.Object, titleGenreRepositoryMock.Object);

            // Act
            IReadOnlyCollection<TitleResponse> response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeEmpty();
            response.Should().HaveCount(5);
            response.Should().BeEquivalentTo(Titles_Action_SciFi_Fantasy);
        }

        private void PopulateAllTitles()
        {
            foreach (var t in this.Titles_Action_SciFi_Fantasy)
            {
                var titleToAdd = new Title(t.Name, t.Description, new List<Review>())
                {
                    Id = t.Id,
                    Genres = t.Genres.Select(g => new TitleGenre()
                    {
                        TitleId = t.Id,
                        GenreId = g.Id,
                        Genre = new Genre()
                        {
                            Id = g.Id,
                            Name = g.Name
                        }
                    }).ToList()
                };

                this.AllTitles.Add(titleToAdd);
            }
            foreach (var t in this.Titles_Drama_SciFi)
            {
                var titleToAdd = new Title(t.Name, t.Description, new List<Review>())
                {
                    Id = t.Id,
                    Genres = t.Genres.Select(g => new TitleGenre()
                    {
                        TitleId = t.Id,
                        GenreId = g.Id,
                        Genre = new Genre()
                        {
                            Id = g.Id,
                            Name = g.Name
                        }
                    }).ToList()
                };

                this.AllTitles.Add(titleToAdd);
            }
            foreach (var t in this.Titles_Romance_Comedy)
            {
                var titleToAdd = new Title(t.Name, t.Description, new List<Review>())
                {
                    Id = t.Id,
                    Genres = t.Genres.Select(g => new TitleGenre()
                    {
                        TitleId = t.Id,
                        GenreId = g.Id,
                        Genre = new Genre()
                        {
                            Id = g.Id,
                            Name = g.Name
                        }
                    }).ToList()
                };

                this.AllTitles.Add(titleToAdd);
            }
        }

        private void PopulateAllTitleGenres()
        {

            foreach (TitleResponse title in Titles_Action_SciFi_Fantasy)
            {
                Title titleToAdd = new Title(title.Name, title.Description, new List<Review>())
                {
                    Id = title.Id
                };

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

                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = action, TitleId = title.Id, GenreId = action.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = sciFi, TitleId = title.Id, GenreId = sciFi.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = fantasy, TitleId = title.Id, GenreId = fantasy.Id });
            }

            foreach (TitleResponse title in Titles_Drama_SciFi)
            {
                Title titleToAdd = new Title(title.Name, title.Description, new List<Review>())
                {
                    Id = title.Id
                };

                Genre drama = new Genre()
                {
                    Id = 18,
                    Name = "Drama"
                };
                Genre sciFi = new Genre()
                {
                    Id = 878,
                    Name = "SciFi"
                };

                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = drama, TitleId = title.Id, GenreId = drama.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = sciFi, TitleId = title.Id, GenreId = sciFi.Id });
            }
            foreach (TitleResponse title in Titles_Romance_Comedy)
            {
                Title titleToAdd = new Title(title.Name, title.Description, new List<Review>())
                {
                    Id = title.Id
                };

                Genre romance = new Genre()
                {
                    Id = 10749,
                    Name = "Romance"
                };
                Genre comedy = new Genre()
                {
                    Id = 35,
                    Name = "Comedy"
                };

                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = romance, TitleId = title.Id, GenreId = romance.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = comedy, TitleId = title.Id, GenreId = comedy.Id });
            }
        }
    }
}
