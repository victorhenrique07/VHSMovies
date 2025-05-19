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
        private List<Person> people;
        private List<Title> titles;
        private List<Genre> genres;

        public DbContextClass CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DbContextClass>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var inMemorySettings = new Dictionary<string, string>();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            people = GetPersonList();
            titles = GetTitlesList();
            genres = GetGenresList();

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
            Context.Titles.RemoveRange(Context.Titles);
            Context.Reviews.RemoveRange(Context.Reviews);

            Context.SaveChanges();
        }

        public void SeedDatabase(DbContextClass Context)
        {
            Context.RecommendedTitles.AddRange(GetRecommendedTitlesList());
            Context.Genres.AddRange(genres);
            Context.Titles.AddRange(titles);
            Context.People.AddRange(people);
            Context.TitlesGenres.AddRange(GetTitlesGenresList());
            Context.Casts.AddRange(GetCastList());
            Context.Reviews.AddRange(GetReviewsList());

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
                    Title = titles.First(t => t.Id == 1297469),
                    Person = people.First(p => p.Id == 1), // Tim Robbins
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 2,
                    Title = titles.First(t => t.Id == 1297469),
                    Person = people.First(p => p.Id == 5),
                    Role = PersonRole.Director
                },
                new Cast
                {
                    Id = 3,
                    Title = titles.First(t => t.Id == 1297469),
                    Person = people.First(p => p.Id == 8), // Stephen King
                    Role = PersonRole.Writer
                },

                // The Dark Knight
                new Cast
                {
                    Id = 4,
                    Title = titles.First(t => t.Id == 1489427),
                    Person = people.First(p => p.Id == 2), // Christian Bale
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 5,
                    Title = titles.First(t => t.Id == 1489427),
                    Person = people.First(p => p.Id == 6), // Christopher Nolan
                    Role = PersonRole.Director
                },
                new Cast
                {
                    Id = 6,
                    Title = titles.First(t => t.Id == 1489427),
                    Person = people.First(p => p.Id == 9), // Jonathan Nolan
                    Role = PersonRole.Writer
                },

                // Breaking Bad
                new Cast
                {
                    Id = 7,
                    Title = titles.First(t => t.Id == 1519096),
                    Person = people.First(p => p.Id == 3), // Bryan Cranston
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 8,
                    Title = titles.First(t => t.Id == 1519096),
                    Person = people.First(p => p.Id == 7), // Vince Gilligan
                    Role = PersonRole.Director
                },

                // Game of Thrones
                new Cast
                {
                    Id = 9,
                    Title = titles.First(t => t.Id == 1522910),
                    Person = people.First(p => p.Id == 4), // Emilia Clarke
                    Role = PersonRole.Actor
                },
                new Cast
                {
                    Id = 10,
                    Title = titles.First(t => t.Id == 1522910),
                    Person = people.First(p => p.Id == 10), // George R. R. Martin
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
        public List<Person> GetPersonList()
        {
            return new List<Person>
            {
                // Atores
                new Person("Tim Robbins") { Id = 1 },         // The Shawshank Redemption
                new Person("Christian Bale") { Id = 2 },      // The Dark Knight
                new Person("Bryan Cranston") { Id = 3 },      // Breaking Bad
                new Person("Emilia Clarke") { Id = 4 },       // Game of Thrones

                // Diretores
                new Person("Frank Darabont") { Id = 5 },      // The Shawshank Redemption
                new Person("Christopher Nolan") { Id = 6 },   // The Dark Knight
                new Person("Vince Gilligan") { Id = 7 },      // Breaking Bad

                // Escritores
                new Person("Stephen King") { Id = 8 },        // The Shawshank Redemption (baseado em obra dele)
                new Person("Jonathan Nolan") { Id = 9 },      // The Dark Knight
                new Person("George R. R. Martin") { Id = 10 }  // Game of Thrones
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
        public List<Review> GetReviewsList()
        {
            return new List<Review>
            {
                new Review("IMDb", 9.3m, 2700000)
                {
                    Id = 1,
                    TitleExternalId = "tt0111161",
                    Title = titles.First(t => t.Id == 1297469)
                },
                new Review("Metacritic", 80.0m, 80_000)
                {
                    Id = 2,
                    TitleExternalId = "mt0111161",
                    Title = titles.First(t => t.Id == 1297469)
                },

                new Review("IMDb", 9.0m, 2600000)
                {
                    Id = 3,
                    TitleExternalId = "tt0468569",
                    Title = titles.First(t => t.Id == 1489427)
                },
                new Review("Metacritic", 84.0m, 85000)
                {
                    Id = 4,
                    TitleExternalId = "mt0468569",
                    Title = titles.First(t => t.Id == 1489427)
                },

                new Review("IMDb", 9.2m, 2000000)
                {
                    Id = 5,
                    TitleExternalId = "tt0068646",
                    Title = titles.First(t => t.Id == 1260356)
                },
                new Review("Metacritic", 100.0m, 75000)
                {
                    Id = 6,
                    TitleExternalId = "mt0068646",
                    Title = titles.First(t => t.Id == 1260356)
                },

                new Review("IMDb", 9.5m, 1900000)
                {
                    Id = 7,
                    TitleExternalId = "tt0903747",
                    Title = titles.First(t => t.Id == 1519096)
                },
                new Review("Metacritic", 87.0m, 60000)
                {
                    Id = 8,
                    TitleExternalId = "mt0903747",
                    Title = titles.First(t => t.Id == 1519096)
                },

                new Review("IMDb", 9.2m, 1900000)
                {
                    Id = 9,
                    TitleExternalId = "tt0944947",
                    Title = titles.First(t => t.Id == 1522910)
                },
                new Review("Metacritic", 86.0m, 65000)
                {
                    Id = 10,
                    TitleExternalId = "mt0944947",
                    Title = titles.First(t => t.Id == 1522910)
                },

                new Review("IMDb", 9.1m, 950000)
                {
                    Id = 11,
                    TitleExternalId = "tt1475582",
                    Title = titles.First(t => t.Id == 1727014)
                },
                new Review("Metacritic", 85.0m, 40000)
                {
                    Id = 12,
                    TitleExternalId = "mt1475582",
                    Title = titles.First(t => t.Id == 1727014)
                },

                new Review("IMDb", 8.9m, 350000)
                {
                    Id = 13,
                    TitleExternalId = "tt11989890",
                    Title = titles.First(t => t.Id == 1613429)
                },
                new Review("Metacritic", 72.0m, 20000)
                {
                    Id = 14,
                    TitleExternalId = "mt11989890",
                    Title = titles.First(t => t.Id == 1613429)
                },

                new Review("IMDb", 8.3m, 100000)
                {
                    Id = 15,
                    TitleExternalId = "tt0060345",
                    Title = titles.First(t => t.Id == 1253249)
                },
                new Review("Metacritic", 74.0m, 9000)
                {
                    Id = 16,
                    TitleExternalId = "mt0060345",
                    Title = titles.First(t => t.Id == 1253249)
                },

                new Review("IMDb", 8.4m, 120000)
                {
                    Id = 17,
                    TitleExternalId = "tt0059026",
                    Title = titles.First(t => t.Id == 1252161)
                },
                new Review("Metacritic", 78.0m, 11000)
                {
                    Id = 18,
                    TitleExternalId = "mt0059026",
                    Title = titles.First(t => t.Id == 1252161)
                },

                new Review("IMDb", 9.4m, 1000000)
                {
                    Id = 19,
                    TitleExternalId = "tt7366338",
                    Title = titles.First(t => t.Id == 2325522)
                },
                new Review("Metacritic", 95.0m, 50000)
                {
                    Id = 20,
                    TitleExternalId = "mt7366338",
                    Title = titles.First(t => t.Id == 2325522)
                },

                new Review("IMDb", 9.4m, 550000)
                {
                    Id = 21,
                    TitleExternalId = "tt0185906",
                    Title = titles.First(t => t.Id == 1342415)
                },
                new Review("Metacritic", 89.0m, 35000)
                {
                    Id = 22,
                    TitleExternalId = "mt0185906",
                    Title = titles.First(t => t.Id == 1342415)
                },

                new Review("IMDb", 9.4m, 250000)
                {
                    Id = 23,
                    TitleExternalId = "tt0795176",
                    Title = titles.First(t => t.Id == 1506162)
                },
                new Review("Metacritic", 90.0m, 22000)
                {
                    Id = 24,
                    TitleExternalId = "mt0795176",
                    Title = titles.First(t => t.Id == 1506162)
                },
            };
        }
        public List<TitleGenre> GetTitlesGenresList()
        {
            return new List<TitleGenre>
            {
                new TitleGenre { Title = titles.First(t => t.Id == 1297469), Genre = genres.First(g => g.Id == 9) },
                new TitleGenre { Title = titles.First(t => t.Id == 1489427), Genre = genres.First(g => g.Id == 1) },
                new TitleGenre { Title = titles.First(t => t.Id == 1489427), Genre = genres.First(g => g.Id == 7) },
                new TitleGenre { Title = titles.First(t => t.Id == 1489427), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 1260356), Genre = genres.First(g => g.Id == 7) },
                new TitleGenre { Title = titles.First(t => t.Id == 1260356), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 1519096), Genre = genres.First(g => g.Id == 7)},
                new TitleGenre { Title = titles.First(t => t.Id == 1519096), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 1519096), Genre = genres.First(g => g.Id == 26)},
                new TitleGenre { Title = titles.First(t => t.Id == 1522910), Genre = genres.First(g => g.Id == 1)},
                new TitleGenre { Title = titles.First(t => t.Id == 1522910), Genre = genres.First(g => g.Id == 3)},
                new TitleGenre { Title = titles.First(t => t.Id == 1522910), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 1727014), Genre = genres.First(g => g.Id == 7)},
                new TitleGenre { Title = titles.First(t => t.Id == 1727014), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 1727014), Genre = genres.First(g => g.Id == 18)},
                new TitleGenre { Title = titles.First(t => t.Id == 1613429), Genre = genres.First(g => g.Id == 5)},
                new TitleGenre { Title = titles.First(t => t.Id == 1613429), Genre = genres.First(g => g.Id == 8)},
                new TitleGenre { Title = titles.First(t => t.Id == 1253249), Genre = genres.First(g => g.Id == 4)},
                new TitleGenre { Title = titles.First(t => t.Id == 1253249), Genre = genres.First(g => g.Id == 6)},
                new TitleGenre { Title = titles.First(t => t.Id == 1253249), Genre = genres.First(g => g.Id == 10)},
                new TitleGenre { Title = titles.First(t => t.Id == 1252161), Genre = genres.First(g => g.Id == 4)},
                new TitleGenre { Title = titles.First(t => t.Id == 1252161), Genre = genres.First(g => g.Id == 6)},
                new TitleGenre { Title = titles.First(t => t.Id == 1252161), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 2325522), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 2325522), Genre = genres.First(g => g.Id == 14)},
                new TitleGenre { Title = titles.First(t => t.Id == 2325522), Genre = genres.First(g => g.Id == 26)},
                new TitleGenre { Title = titles.First(t => t.Id == 1342415), Genre = genres.First(g => g.Id == 1)},
                new TitleGenre { Title = titles.First(t => t.Id == 1342415), Genre = genres.First(g => g.Id == 9)},
                new TitleGenre { Title = titles.First(t => t.Id == 1342415), Genre = genres.First(g => g.Id == 14)},
                new TitleGenre { Title = titles.First(t => t.Id == 1506162), Genre = genres.First(g => g.Id == 8)},
                new TitleGenre { Title = titles.First(t => t.Id == 1506162), Genre = genres.First(g => g.Id == 10)}
            };
        }
    }
}