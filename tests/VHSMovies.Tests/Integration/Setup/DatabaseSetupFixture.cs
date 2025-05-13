using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure;

namespace VHSMovies.Tests.Integration.Setup
{
    public class DatabaseSetupFixture
    {
        public DbContextClass CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DbContextClass>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var inMemorySettings = new Dictionary<string, string>();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new DbContextClass(configuration, options);
        }

        public void Dispose(DbContextClass Context)
        {
            Context.Dispose();
        }

        public void ClearAllTables(DbContextClass Context)
        {
            Context.RecommendedTitles.RemoveRange(Context.RecommendedTitles);
            Context.People.RemoveRange(Context.People);
            Context.Genres.RemoveRange(Context.Genres);
            Context.Casts.RemoveRange(Context.Casts);

            Context.SaveChanges();
        }

        public void SeedDatabase(DbContextClass Context)
        {
            Context.RecommendedTitles.AddRange(GetRecommendedTitlesList());
            Context.People.AddRange(GetPersonList());
            Context.Genres.AddRange(GetGenresList());
            Context.Casts.AddRange(GetCastList());
            Context.Titles.AddRange(GetTitlesList());

            Context.SaveChanges();
        }

        public List<Cast> GetCastList()
        {
            return new List<Cast>
            {
                // The Shawshank Redemption
                new Cast
                {
                    Id = 1,
                    TitleId = 1297469,
                    PersonId = 1, // Tim Robbins
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 2,
                    TitleId = 1297469,
                    PersonId = 5, // Frank Darabont
                    Role = PersonRole.Director
                },
                new Cast
                {
                    Id = 3,
                    TitleId = 1297469,
                    PersonId = 8, // Stephen King
                    Role = PersonRole.Writer
                },

                // The Dark Knight
                new Cast
                {
                    Id = 4,
                    TitleId = 1489427,
                    PersonId = 2, // Christian Bale
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 5,
                    TitleId = 1489427,
                    PersonId = 6, // Christopher Nolan
                    Role = PersonRole.Director
                },
                new Cast
                {
                    Id = 6,
                    TitleId = 1489427,
                    PersonId = 9, // Jonathan Nolan
                    Role = PersonRole.Writer
                },

                // Breaking Bad
                new Cast
                {
                    Id = 7,
                    TitleId = 1519096,
                    PersonId = 3, // Bryan Cranston
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 8,
                    TitleId = 1519096,
                    PersonId = 7, // Vince Gilligan
                    Role = PersonRole.Director
                },

                // Game of Thrones
                new Cast
                {
                    Id = 9,
                    TitleId = 1522910,
                    PersonId = 4, // Emilia Clarke
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 10,
                    TitleId = 1522910,
                    PersonId = 10, // George R. R. Martin
                    Role = PersonRole.Writer
                }
            };
        }
        public List<Genre> GetGenresList()
        {
            return new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Adult" },
                new Genre { Id = 3, Name = "Adventure" },
                new Genre { Id = 4, Name = "Animation" },
                new Genre { Id = 5, Name = "Biography" },
                new Genre { Id = 6, Name = "Comedy" },
                new Genre { Id = 7, Name = "Crime" },
                new Genre { Id = 8, Name = "Documentary" },
                new Genre { Id = 9, Name = "Drama" },
                new Genre { Id = 10, Name = "Family" },
                new Genre { Id = 11, Name = "Fantasy" },
                new Genre { Id = 12, Name = "Film Noir" },
                new Genre { Id = 13, Name = "Game Show" },
                new Genre { Id = 14, Name = "History" },
                new Genre { Id = 15, Name = "Horror" },
                new Genre { Id = 16, Name = "Musical" },
                new Genre { Id = 17, Name = "Music" },
                new Genre { Id = 18, Name = "Mystery" },
                new Genre { Id = 20, Name = "Reality-TV" },
                new Genre { Id = 21, Name = "Romance" },
                new Genre { Id = 22, Name = "Sci-Fi" },
                new Genre { Id = 24, Name = "Sport" },
                new Genre { Id = 25, Name = "Talk-Show" },
                new Genre { Id = 26, Name = "Thriller" },
                new Genre { Id = 27, Name = "War" },
                new Genre { Id = 28, Name = "Western" },
            };
        }
        public List<Person> GetPersonList()
        {
            return new List<Person>
            {
                // Atores
                new Person("Tim Robbins"),         // The Shawshank Redemption
                new Person("Christian Bale"),      // The Dark Knight
                new Person("Bryan Cranston"),      // Breaking Bad
                new Person("Emilia Clarke"),       // Game of Thrones

                // Diretores
                new Person("Frank Darabont"),      // The Shawshank Redemption
                new Person("Christopher Nolan"),   // The Dark Knight
                new Person("Vince Gilligan"),      // Breaking Bad

                // Escritores
                new Person("Stephen King"),        // The Shawshank Redemption (baseado em obra dele)
                new Person("Jonathan Nolan"),      // The Dark Knight
                new Person("George R. R. Martin")  // Game of Thrones
            };
        }
        public List<RecommendedTitle> GetRecommendedTitlesList()
        {
            return new List<RecommendedTitle>
            {
                new RecommendedTitle
                {
                    Id = 1297469,
                    IMDB_Id = "tt0111161",
                    Name = "The Shawshank Redemption",
                    Type = 1,
                    ReleaseDate = 1994,
                    AverageRating = 9.3m,
                    TotalReviews = 6079614,
                    Genres = new[] { "Drama" },
                    Relevance = 11.13m
                },
                new RecommendedTitle
                {
                    Id = 1489427,
                    IMDB_Id = "tt0468569",
                    Name = "The Dark Knight",
                    Type = 1,
                    ReleaseDate = 2008,
                    AverageRating = 9.0m,
                    TotalReviews = 6033212,
                    Genres = new[] { "Action", "Crime", "Drama" },
                    Relevance = 10.98m
                },
                new RecommendedTitle
                {
                    Id = 1260356,
                    IMDB_Id = "tt0068646",
                    Name = "The Godfather",
                    Type = 1,
                    ReleaseDate = 1972,
                    AverageRating = 9.2m,
                    TotalReviews = 4243816,
                    Genres = new[] { "Crime", "Drama" },
                    Relevance = 10.93m
                },
                new RecommendedTitle
                {
                    Id = 1519096,
                    IMDB_Id = "tt0903747",
                    Name = "Breaking Bad",
                    Type = 2,
                    ReleaseDate = 2008,
                    AverageRating = 9.5m,
                    TotalReviews = 4650530,
                    Genres = new[] { "Crime", "Drama", "Thriller" },
                    Relevance = 11.12m
                },
                new RecommendedTitle
                {
                    Id = 1522910,
                    IMDB_Id = "tt0944947",
                    Name = "Game of Thrones",
                    Type = 2,
                    ReleaseDate = 2011,
                    AverageRating = 9.2m,
                    TotalReviews = 4859238,
                    Genres = new[] { "Action", "Adventure", "Drama" },
                    Relevance = 10.99m
                },
                new RecommendedTitle
                {
                    Id = 1727014,
                    IMDB_Id = "tt1475582",
                    Name = "Sherlock",
                    Type = 2,
                    ReleaseDate = 2010,
                    AverageRating = 9.0m,
                    TotalReviews = 2093674,
                    Genres = new[] { "Crime", "Drama", "Mystery" },
                    Relevance = 10.52m
                },
                new RecommendedTitle
                {
                    Id = 1613429,
                    IMDB_Id = "tt11989890",
                    Name = "David Attenborough: A Life on Our Planet",
                    Type = 3,
                    ReleaseDate = 2020,
                    AverageRating = 8.9m,
                    TotalReviews = 70738,
                    Genres = new[] { "Biography", "Documentary" },
                    Relevance = 9.00m
                },
                new RecommendedTitle
                {
                    Id = 1253249,
                    IMDB_Id = "tt0060345",
                    Name = "How the Grinch Stole Christmas!",
                    Type = 3,
                    ReleaseDate = 1966,
                    AverageRating = 8.3m,
                    TotalReviews = 125664,
                    Genres = new[] { "Animation", "Comedy", "Family" },
                    Relevance = 8.95m
                },
                new RecommendedTitle
                {
                    Id = 1252161,
                    IMDB_Id = "tt0059026",
                    Name = "A Charlie Brown Christmas",
                    Type = 3,
                    ReleaseDate = 1965,
                    AverageRating = 8.3m,
                    TotalReviews = 94586,
                    Genres = new[] { "Animation", "Comedy", "Drama" },
                    Relevance = 8.82m
                },
                new RecommendedTitle
                {
                    Id = 2325522,
                    IMDB_Id = "tt7366338",
                    Name = "Chernobyl",
                    Type = 4,
                    ReleaseDate = 2019,
                    AverageRating = 9.3m,
                    TotalReviews = 1894754,
                    Genres = new[] { "Drama", "History", "Thriller" },
                    Relevance = 10.63m
                },
                new RecommendedTitle
                {
                    Id = 1342415,
                    IMDB_Id = "tt0185906",
                    Name = "Band of Brothers",
                    Type = 4,
                    ReleaseDate = 2001,
                    AverageRating = 9.4m,
                    TotalReviews = 1122022,
                    Genres = new[] { "Action", "Drama", "History" },
                    Relevance = 10.45m
                },
                new RecommendedTitle
                {
                    Id = 1506162,
                    IMDB_Id = "tt0795176",
                    Name = "Planet Earth",
                    Type = 4,
                    ReleaseDate = 2006,
                    AverageRating = 9.4m,
                    TotalReviews = 454458,
                    Genres = new[] { "Documentary", "Family" },
                    Relevance = 10.06m
                }
            };
        }
        public List<Title> GetTitlesList()
        {
            return new List<Title>
            {
                new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161")
                {
                    Id = 1297469,
                    Relevance = 11.13m
                },
                new Title("The Dark Knight", TitleType.Movie, 2008, "tt0468569")
                {
                    Id = 1489427,
                    Relevance = 10.98m
                },
                new Title("The Godfather", TitleType.Movie, 1972, "tt0068646")
                {
                    Id = 1260356,
                    Relevance = 10.93m
                },
                new Title("Breaking Bad", TitleType.TvSeries, 2008, "tt0903747")
                {
                    Id = 1519096,
                    Relevance = 11.12m
                },
                new Title("Game of Thrones", TitleType.TvSeries, 2011, "tt0944947")
                {
                    Id = 1522910,
                    Relevance = 10.99m
                },
                new Title("Sherlock", TitleType.TvSeries, 2010, "tt1475582")
                {
                    Id = 1727014,
                    Relevance = 10.52m
                },
                new Title("David Attenborough: A Life on Our Planet", TitleType.TvMovie, 2020, "tt11989890")
                {
                    Id = 1613429,
                    Relevance = 9.00m
                },
                new Title("How the Grinch Stole Christmas!", TitleType.TvMovie, 1966, "tt0060345")
                {
                    Id = 1253249,
                    Relevance = 8.95m
                },
                new Title("A Charlie Brown Christmas", TitleType.TvMovie, 1965, "tt0059026")
                {
                    Id = 1252161,
                    Relevance = 8.82m
                },
                new Title("Chernobyl", TitleType.TvMiniSeries, 2019, "tt7366338")
                {
                    Id = 2325522,
                    Relevance = 10.63m
                },
                new Title("Band of Brothers", TitleType.TvMiniSeries, 2001, "tt0185906")
                {
                    Id = 1342415,
                    Relevance = 10.45m
                },
                new Title("Planet Earth", TitleType.TvMiniSeries, 2006, "tt0795176")
                {
                    Id = 1506162,
                    Relevance = 10.06m
                }
            };
        }
    }
}