using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Tests.Setup
{
    public class BaseTestsFixture : IDisposable
    {
        public readonly Mock<IRecommendedTitlesRepository> recommendedTitlesMock;
        public readonly Mock<IGenreRepository> genresRepositoryMock;

        public readonly IReadOnlyCollection<RecommendedTitle> TitlesActionSciFiFantasy;
        public readonly IReadOnlyCollection<RecommendedTitle> TitlesDramaSciFiThriller;
        public readonly IReadOnlyCollection<RecommendedTitle> TitlesRomanceComedyThriller;
        public readonly List<RecommendedTitle> AllTitles = new List<RecommendedTitle>();
        public readonly IReadOnlyCollection<Genre> AllGenres;

        public BaseTestsFixture()
        {
            recommendedTitlesMock = new();
            genresRepositoryMock = new();
            var fixture = new Fixture();

            AllGenres = new List<Genre>
            {
                new Genre() { Id = 1, Name = "Action" },
                new Genre() { Id = 2, Name = "Adventure" },
                new Genre() { Id = 3, Name = "Animation" },
                new Genre() { Id = 4, Name = "Comedy" },
                new Genre() { Id = 5, Name = "Crime" },
                new Genre() { Id = 6, Name = "Documentary" },
                new Genre() { Id = 7, Name = "Drama" },
                new Genre() { Id = 8, Name = "Family" },
                new Genre() { Id = 9, Name = "Fantasy" },
                new Genre() { Id = 10, Name = "History" },
                new Genre() { Id = 11, Name = "Horror" },
                new Genre() { Id = 12, Name = "Music" },
                new Genre() { Id = 13, Name = "Mystery" },
                new Genre() { Id = 14, Name = "Romance" },
                new Genre() { Id = 15, Name = "SciFi" },
                new Genre() { Id = 16, Name = "TV Movie" },
                new Genre() { Id = 17, Name = "Thriller" },
                new Genre() { Id = 18, Name = "War" },
                new Genre() { Id = 19, Name = "Western" }
            };

            fixture.Customize<DateOnly>(c => c.FromFactory(() => new DateOnly(2023, 1, 1)));

            fixture.Customize<RecommendedTitle>(composer =>
                composer.With(x => x.Genres, new[] { "Action", "SciFi", "Fantasy" }));

            TitlesActionSciFiFantasy = fixture.CreateMany<RecommendedTitle>(20).ToList();

            fixture.Customize<RecommendedTitle>(composer =>
                composer.With(x => x.Genres, new[] { "Drama", "SciFi", "Thriller" }));

            TitlesDramaSciFiThriller = fixture.CreateMany<RecommendedTitle>(20).ToList();

            fixture.Customize<RecommendedTitle>(composer =>
                composer.With(x => x.Genres, new[] { "Romance", "Comedy", "Drama" }));

            TitlesRomanceComedyThriller = fixture.CreateMany<RecommendedTitle>(20).ToList();

            AllTitles.AddRange(TitlesActionSciFiFantasy);
            AllTitles.AddRange(TitlesDramaSciFiThriller);
            AllTitles.AddRange(TitlesRomanceComedyThriller);

            genresRepositoryMock
                .Setup(g => g.GetAll())
                .ReturnsAsync(AllGenres);

            recommendedTitlesMock
                .Setup(r => r.Query())
                .Returns(AllTitles.AsQueryable());
        }

        public void Dispose()
        {
        }
    }
}
