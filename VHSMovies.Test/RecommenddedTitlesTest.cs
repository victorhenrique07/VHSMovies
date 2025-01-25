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
                new GenreResponse(878, "SciFi"),
                new GenreResponse(53, "Thriller")
            }));

            this.Titles_Drama_SciFi_Thriller = fixture.CreateMany<TitleResponse>(5).ToList();

            fixture.Customize<TitleResponse>(c => c.With(t => t.Genres, new List<GenreResponse>
            {
                new GenreResponse(10749, "Romance"),
                new GenreResponse(35, "Comedy"),
                new GenreResponse(18, "Drama")
            }));

            this.Titles_Romance_Comedy_Thriller = fixture.CreateMany<TitleResponse>(5).ToList();

            PopulateAllTitles();
            PopulateAllTitleGenres();
        }

        private readonly IReadOnlyCollection<TitleResponse> Titles_Action_SciFi_Fantasy;
        private readonly IReadOnlyCollection<TitleResponse> Titles_Drama_SciFi_Thriller;
        private readonly IReadOnlyCollection<TitleResponse> Titles_Romance_Comedy_Thriller;

        private readonly IList<Title> AllTitles = new List<Title>();
        private readonly IList<TitleGenre> AllTitlesGenres = new List<TitleGenre>();

        [Fact]
        public async Task Test_If_RecommendedTitlesQueryHandler_Is_Returning_5_Titles_With_Action_SciFi_Fantasy_Genres()
        {
            // Arrange
            int[] includeGenres = [14, 28, 878];
            int[] excludeGenres = [18, 35, 53, 10749];

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres,
                ExcludeGenres = excludeGenres
            };

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
            response.Should().NotBeEquivalentTo(Titles_Romance_Comedy_Thriller);
            response.Should().NotBeEquivalentTo(Titles_Drama_SciFi_Thriller);
        }

        [Fact]
        public async Task Test_If_RecommendedTitlesQueryHandler_Is_Returning_5_Titles_With_Drama_SciFi_Thriller_Genres()
        {
            // Arrange
            int[] includeGenres = [18, 878, 53];
            int[] excludeGenres = [14, 28, 10749, 35];

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres,
                ExcludeGenres = excludeGenres
            };

            titleRepositoryMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(AllTitles);

            var handler = new GetRecommendedTitlesQueryHandler(titleRepositoryMock.Object, castRepositoryMock.Object, titleGenreRepositoryMock.Object);

            // Act
            IReadOnlyCollection<TitleResponse> response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeEmpty();
            response.Should().HaveCount(5);
            response.Should().BeEquivalentTo(Titles_Drama_SciFi_Thriller);
            response.Should().NotBeEquivalentTo(Titles_Romance_Comedy_Thriller);
            response.Should().NotBeEquivalentTo(Titles_Action_SciFi_Fantasy);
        }

        [Fact]
        public async Task Test_If_RecommendedTitlesQueryHandler_Is_Returning_5_Titles_With_Romance_Comedy_Genres()
        {
            // Arrange
            int[] includeGenres = [18, 35, 10749];
            int[] excludeGenres = [14, 28, 878];

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres,
                ExcludeGenres = excludeGenres
            };

            titleRepositoryMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(AllTitles);

            var handler = new GetRecommendedTitlesQueryHandler(titleRepositoryMock.Object, castRepositoryMock.Object, titleGenreRepositoryMock.Object);

            // Act
            IReadOnlyCollection<TitleResponse> response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeEmpty();
            response.Should().HaveCount(5);
            response.Should().BeEquivalentTo(Titles_Romance_Comedy_Thriller);
            response.Should().NotBeEquivalentTo(Titles_Drama_SciFi_Thriller);
            response.Should().NotBeEquivalentTo(Titles_Action_SciFi_Fantasy);

        }

        [Fact]
        public async Task Test_If_RecommendedTitlesQueryHandler_Is_Returning_10_Titles_With_Drama_Genres()
        {
            // Arrange
            int[] includeGenres = [18];
            int[] excludeGenres = [14, 28];

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres,
                ExcludeGenres = excludeGenres
            };

            titleRepositoryMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(AllTitles);

            var handler = new GetRecommendedTitlesQueryHandler(titleRepositoryMock.Object, castRepositoryMock.Object, titleGenreRepositoryMock.Object);

            // Act
            IReadOnlyCollection<TitleResponse> response = await handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().NotBeEmpty();
            response.Should().HaveCount(10);
            response.Should().Contain(t => Titles_Romance_Comedy_Thriller.Any(x => x.Id == t.Id));
            response.Should().Contain(t => Titles_Drama_SciFi_Thriller.Any(x => x.Id == t.Id));
            response.Should().NotContain(t => Titles_Action_SciFi_Fantasy.Any(x => x.Id == t.Id));
        }

        private void PopulateAllTitles()
        {
            foreach (var t in this.Titles_Action_SciFi_Fantasy)
            {
                var titleToAdd = new Title(t.Name, t.Description, "", "", new List<Review>())
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
            foreach (var t in this.Titles_Drama_SciFi_Thriller)
            {
                var titleToAdd = new Title(t.Name, t.Description, "", "", new List<Review>())
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
            foreach (var t in this.Titles_Romance_Comedy_Thriller)
            {
                var titleToAdd = new Title(t.Name, t.Description, "", "", new List<Review>())
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
                Title titleToAdd = new Title(title.Name, title.Description, "", "", new List<Review>())
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

            foreach (TitleResponse title in Titles_Drama_SciFi_Thriller)
            {
                Title titleToAdd = new Title(title.Name, title.Description, "", "", new List<Review>())
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
                Genre thriller = new Genre()
                {
                    Id = 53,
                    Name = "Thriller"
                };

                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = drama, TitleId = title.Id, GenreId = drama.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = sciFi, TitleId = title.Id, GenreId = sciFi.Id });
            }
            foreach (TitleResponse title in Titles_Romance_Comedy_Thriller)
            {
                Title titleToAdd = new Title(title.Name, title.Description, "", "", new List<Review>())
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
                Genre drama = new Genre()
                {
                    Id = 18,
                    Name = "Drama"
                };
                

                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = romance, TitleId = title.Id, GenreId = romance.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = comedy, TitleId = title.Id, GenreId = comedy.Id });
                AllTitlesGenres.Add(new TitleGenre { Title = titleToAdd, Genre = drama, TitleId = title.Id, GenreId = drama.Id });
            }
        }
    }
}
