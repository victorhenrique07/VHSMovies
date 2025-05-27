using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure;

namespace VHSMovies.Tests.Integration.Setup
{
    public class PopulateDatabase
    {

        public void SeedDatabase(DbContextClass Context)
        {
            Context.People.AddRange(GetPersonList(Context));
            Context.Genres.AddRange(GetGenresList(Context));
            Context.Titles.AddRange(GetTitlesList(Context));
            Context.RecommendedTitles.AddRange(GetRecommendedTitlesList(Context));

            Context.SaveChanges();

            Context.Casts.AddRange(GetCastList(Context));
            Context.TitlesGenres.AddRange(GetTitlesGenresList(Context));
            Context.Reviews.AddRange(GetReviewsList(Context));

            Context.SaveChanges();
        }

        public List<Cast> GetCastList(DbContextClass Context)
        {
            return new List<Cast>
            {
                // The Shawshank Redemption
                new Cast { Id = 1, Title = Context.Titles.First(t => t.Id == 1297469), Person = Context.People.First(p => p.Id == 1), Role = PersonRole.Actor },
                new Cast { Id = 2, Title = Context.Titles.First(t => t.Id == 1297469), Person = Context.People.First(p => p.Id == 5), Role = PersonRole.Director },
                new Cast { Id = 3, Title = Context.Titles.First(t => t.Id == 1297469), Person = Context.People.First(p => p.Id == 8), Role = PersonRole.Writer },

                // The Dark Knight
                new Cast { Id = 4, Title = Context.Titles.First(t => t.Id == 1489427), Person = Context.People.First(p => p.Id == 2), Role = PersonRole.Actor },
                new Cast { Id = 5, Title = Context.Titles.First(t => t.Id == 1489427), Person = Context.People.First(p => p.Id == 6), Role = PersonRole.Director },
                new Cast { Id = 6, Title = Context.Titles.First(t => t.Id == 1489427), Person = Context.People.First(p => p.Id == 9), Role = PersonRole.Writer },

                // Breaking Bad
                new Cast { Id = 7, Title = Context.Titles.First(t => t.Id == 1519096), Person = Context.People.First(p => p.Id == 3), Role = PersonRole.Actor },
                new Cast { Id = 8, Title = Context.Titles.First(t => t.Id == 1519096), Person = Context.People.First(p => p.Id == 7), Role = PersonRole.Director },
                new Cast { Id = 9, Title = Context.Titles.First(t => t.Id == 1519096), Person = Context.People.First(p => p.Id == 16), Role = PersonRole.Writer },

                // Game of Thrones
                new Cast { Id = 10, Title = Context.Titles.First(t => t.Id == 1522910), Person = Context.People.First(p => p.Id == 4), Role = PersonRole.Actor },
                new Cast { Id = 11, Title = Context.Titles.First(t => t.Id == 1522910), Person = Context.People.First(p => p.Id == 6), Role = PersonRole.Director },
                new Cast { Id = 12, Title = Context.Titles.First(t => t.Id == 1522910), Person = Context.People.First(p => p.Id == 10), Role = PersonRole.Writer },

                // Sherlock
                new Cast { Id = 13, Title = Context.Titles.First(t => t.Id == 1727014), Person = Context.People.First(p => p.Id == 4), Role = PersonRole.Actor },
                new Cast { Id = 14, Title = Context.Titles.First(t => t.Id == 1727014), Person = Context.People.First(p => p.Id == 6), Role = PersonRole.Director },
                new Cast { Id = 15, Title = Context.Titles.First(t => t.Id == 1727014), Person = Context.People.First(p => p.Id == 9), Role = PersonRole.Writer },

                // David Attenborough: A Life on Our Planet
                new Cast { Id = 16, Title = Context.Titles.First(t => t.Id == 1613429), Person = Context.People.First(p => p.Id == 12), Role = PersonRole.Actor },
                new Cast { Id = 17, Title = Context.Titles.First(t => t.Id == 1613429), Person = Context.People.First(p => p.Id == 12), Role = PersonRole.Director },
                new Cast { Id = 18, Title = Context.Titles.First(t => t.Id == 1613429), Person = Context.People.First(p => p.Id == 20), Role = PersonRole.Writer },

                // How the Grinch Stole Christmas!
                new Cast { Id = 19, Title = Context.Titles.First(t => t.Id == 1253249), Person = Context.People.First(p => p.Id == 1), Role = PersonRole.Actor },
                new Cast { Id = 20, Title = Context.Titles.First(t => t.Id == 1253249), Person = Context.People.First(p => p.Id == 13), Role = PersonRole.Director },
                new Cast { Id = 21, Title = Context.Titles.First(t => t.Id == 1253249), Person = Context.People.First(p => p.Id == 17), Role = PersonRole.Writer },

                // A Charlie Brown Christmas
                new Cast { Id = 22, Title = Context.Titles.First(t => t.Id == 1252161), Person = Context.People.First(p => p.Id == 1), Role = PersonRole.Actor },
                new Cast { Id = 23, Title = Context.Titles.First(t => t.Id == 1252161), Person = Context.People.First(p => p.Id == 14), Role = PersonRole.Director },
                new Cast { Id = 24, Title = Context.Titles.First(t => t.Id == 1252161), Person = Context.People.First(p => p.Id == 18), Role = PersonRole.Writer },

                // Chernobyl
                new Cast { Id = 25, Title = Context.Titles.First(t => t.Id == 2325522), Person = Context.People.First(p => p.Id == 3), Role = PersonRole.Actor },
                new Cast { Id = 26, Title = Context.Titles.First(t => t.Id == 2325522), Person = Context.People.First(p => p.Id == 11), Role = PersonRole.Director },
                new Cast { Id = 27, Title = Context.Titles.First(t => t.Id == 2325522), Person = Context.People.First(p => p.Id == 19), Role = PersonRole.Writer },

                // Band of Brothers
                new Cast { Id = 28, Title = Context.Titles.First(t => t.Id == 1342415), Person = Context.People.First(p => p.Id == 3), Role = PersonRole.Actor },
                new Cast { Id = 29, Title = Context.Titles.First(t => t.Id == 1342415), Person = Context.People.First(p => p.Id == 15), Role = PersonRole.Director },
                new Cast { Id = 30, Title = Context.Titles.First(t => t.Id == 1342415), Person = Context.People.First(p => p.Id == 21), Role = PersonRole.Writer },

                // Planet Earth
                new Cast { Id = 31, Title = Context.Titles.First(t => t.Id == 1506162), Person = Context.People.First(p => p.Id == 12), Role = PersonRole.Actor },
                new Cast { Id = 32, Title = Context.Titles.First(t => t.Id == 1506162), Person = Context.People.First(p => p.Id == 12), Role = PersonRole.Director },
                new Cast { Id = 33, Title = Context.Titles.First(t => t.Id == 1506162), Person = Context.People.First(p => p.Id == 20), Role = PersonRole.Writer }
            };
        }
        public List<Title> GetTitlesList(DbContextClass Context)
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
                new Title("David Attenborough: A Life on Our Planet", TitleType.Movie, 2020, "tt11989890")
                {
                    Id = 1613429,
                    Relevance = 9.00m
                },
                new Title("How the Grinch Stole Christmas!", TitleType.Movie, 1966, "tt0060345")
                {
                    Id = 1253249,
                    Relevance = 8.95m
                },
                new Title("A Charlie Brown Christmas", TitleType.Movie, 1965, "tt0059026")
                {
                    Id = 1252161,
                    Relevance = 8.82m
                },
                new Title("Chernobyl", TitleType.TvSeries, 2019, "tt7366338")
                {
                    Id = 2325522,
                    Relevance = 10.63m
                },
                new Title("Band of Brothers", TitleType.TvSeries, 2001, "tt0185906")
                {
                    Id = 1342415,
                    Relevance = 10.45m
                },
                new Title("Planet Earth", TitleType.TvSeries, 2006, "tt0795176")
                {
                    Id = 1506162,
                    Relevance = 10.06m
                }
            };
        }
        public List<Person> GetPersonList(DbContextClass Context)
        {
            return new List<Person>
            {
                // Atores
                new Person("Tim Robbins") { Id = 1, IMDB_Id = "tt1" },
                new Person("Christian Bale") { Id = 2, IMDB_Id = "tt2" },
                new Person("Bryan Cranston") { Id = 3, IMDB_Id = "tt3" },
                new Person("Emilia Clarke") { Id = 4, IMDB_Id = "tt4" },

                // Diretores
                new Person("Frank Darabont") { Id = 5, IMDB_Id = "tt5" },
                new Person("Christopher Nolan") { Id = 6, IMDB_Id = "tt6" },
                new Person("Vince Gilligan") { Id = 7, IMDB_Id = "tt7" },
                new Person("Johan Renck") { Id = 11, IMDB_Id = "tt11" }, // Chernobyl
                new Person("David Attenborough") { Id = 12, IMDB_Id = "tt12" }, // Documentários
                new Person("Ron Howard") { Id = 13, IMDB_Id = "tt13" }, // How the Grinch Stole Christmas!
                new Person("Bill Melendez") { Id = 14, IMDB_Id = "tt14" }, // A Charlie Brown Christmas
                new Person("Tom Hanks") { Id = 15, IMDB_Id = "tt15" }, // Band of Brothers

                // Escritores
                new Person("Stephen King") { Id = 8, IMDB_Id = "tt8" },
                new Person("Jonathan Nolan") { Id = 9, IMDB_Id = "tt9" },
                new Person("George R. R. Martin") { Id = 10, IMDB_Id = "tt10" },
                new Person("Andrew Stanton") { Id = 16, IMDB_Id = "tt16" }, // Wall-E
                new Person("Dr. Seuss") { Id = 17, IMDB_Id = "tt17" }, // How the Grinch Stole Christmas!
                new Person("Charles M. Schulz") { Id = 18, IMDB_Id = "tt18" }, // Peanuts
                new Person("Craig Mazin") { Id = 19, IMDB_Id = "tt19" }, // Chernobyl
                new Person("Mark Olshaker") { Id = 20, IMDB_Id = "tt20" }, // Documentários
                new Person("Erik Durschmied") { Id = 21, IMDB_Id = "tt21" } // Band of Brothers
            };
        }
        public List<Genre> GetGenresList(DbContextClass Context)
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
        public List<RecommendedTitle> GetRecommendedTitlesList(DbContextClass Context)
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
        public List<Review> GetReviewsList(DbContextClass Context)
        {
            return new List<Review>
            {
                new Review("IMDb", 9.3m, 2700000)
                {
                    Id = 1,
                    TitleExternalId = "tt0111161",
                    Title = Context.Titles.First(t => t.Id == 1297469)
                },
                new Review("Metacritic", 80.0m, 80_000)
                {
                    Id = 2,
                    TitleExternalId = "mt0111161",
                    Title = Context.Titles.First(t => t.Id == 1297469)
                },

                new Review("IMDb", 9.0m, 2600000)
                {
                    Id = 3,
                    TitleExternalId = "tt0468569",
                    Title = Context.Titles.First(t => t.Id == 1489427)
                },
                new Review("Metacritic", 84.0m, 85000)
                {
                    Id = 4,
                    TitleExternalId = "mt0468569",
                    Title = Context.Titles.First(t => t.Id == 1489427)
                },

                new Review("IMDb", 9.2m, 2000000)
                {
                    Id = 5,
                    TitleExternalId = "tt0068646",
                    Title = Context.Titles.First(t => t.Id == 1260356)
                },
                new Review("Metacritic", 100.0m, 75000)
                {
                    Id = 6,
                    TitleExternalId = "mt0068646",
                    Title = Context.Titles.First(t => t.Id == 1260356)
                },

                new Review("IMDb", 9.5m, 1900000)
                {
                    Id = 7,
                    TitleExternalId = "tt0903747",
                    Title = Context.Titles.First(t => t.Id == 1519096)
                },
                new Review("Metacritic", 87.0m, 60000)
                {
                    Id = 8,
                    TitleExternalId = "mt0903747",
                    Title = Context.Titles.First(t => t.Id == 1519096)
                },

                new Review("IMDb", 9.2m, 1900000)
                {
                    Id = 9,
                    TitleExternalId = "tt0944947",
                    Title = Context.Titles.First(t => t.Id == 1522910)
                },
                new Review("Metacritic", 86.0m, 65000)
                {
                    Id = 10,
                    TitleExternalId = "mt0944947",
                    Title = Context.Titles.First(t => t.Id == 1522910)
                },

                new Review("IMDb", 9.1m, 950000)
                {
                    Id = 11,
                    TitleExternalId = "tt1475582",
                    Title = Context.Titles.First(t => t.Id == 1727014)
                },
                new Review("Metacritic", 85.0m, 40000)
                {
                    Id = 12,
                    TitleExternalId = "mt1475582",
                    Title = Context.Titles.First(t => t.Id == 1727014)
                },

                new Review("IMDb", 8.9m, 350000)
                {
                    Id = 13,
                    TitleExternalId = "tt11989890",
                    Title = Context.Titles.First(t => t.Id == 1613429)
                },
                new Review("Metacritic", 72.0m, 20000)
                {
                    Id = 14,
                    TitleExternalId = "mt11989890",
                    Title = Context.Titles.First(t => t.Id == 1613429)
                },

                new Review("IMDb", 8.3m, 100000)
                {
                    Id = 15,
                    TitleExternalId = "tt0060345",
                    Title = Context.Titles.First(t => t.Id == 1253249)
                },
                new Review("Metacritic", 74.0m, 9000)
                {
                    Id = 16,
                    TitleExternalId = "mt0060345",
                    Title = Context.Titles.First(t => t.Id == 1253249)
                },

                new Review("IMDb", 8.4m, 120000)
                {
                    Id = 17,
                    TitleExternalId = "tt0059026",
                    Title = Context.Titles.First(t => t.Id == 1252161)
                },
                new Review("Metacritic", 78.0m, 11000)
                {
                    Id = 18,
                    TitleExternalId = "mt0059026",
                    Title = Context.Titles.First(t => t.Id == 1252161)
                },

                new Review("IMDb", 9.4m, 1000000)
                {
                    Id = 19,
                    TitleExternalId = "tt7366338",
                    Title = Context.Titles.First(t => t.Id == 2325522)
                },
                new Review("Metacritic", 95.0m, 50000)
                {
                    Id = 20,
                    TitleExternalId = "mt7366338",
                    Title = Context.Titles.First(t => t.Id == 2325522)
                },

                new Review("IMDb", 9.4m, 550000)
                {
                    Id = 21,
                    TitleExternalId = "tt0185906",
                    Title = Context.Titles.First(t => t.Id == 1342415)
                },
                new Review("Metacritic", 89.0m, 35000)
                {
                    Id = 22,
                    TitleExternalId = "mt0185906",
                    Title = Context.Titles.First(t => t.Id == 1342415)
                },

                new Review("IMDb", 9.4m, 250000)
                {
                    Id = 23,
                    TitleExternalId = "tt0795176",
                    Title = Context.Titles.First(t => t.Id == 1506162)
                },
                new Review("Metacritic", 90.0m, 22000)
                {
                    Id = 24,
                    TitleExternalId = "mt0795176",
                    Title = Context.Titles.First(t => t.Id == 1506162)
                },
            };
        }
        public List<TitleGenre> GetTitlesGenresList(DbContextClass Context)
        {
            return new List<TitleGenre>
            {
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1297469), Genre = Context.Genres.First(g => g.Id == 9) },
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1489427), Genre = Context.Genres.First(g => g.Id == 1) },
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1489427), Genre = Context.Genres.First(g => g.Id == 7) },
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1489427), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1260356), Genre = Context.Genres.First(g => g.Id == 7) },
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1260356), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1519096), Genre = Context.Genres.First(g => g.Id == 7)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1519096), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1519096), Genre = Context.Genres.First(g => g.Id == 26)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1522910), Genre = Context.Genres.First(g => g.Id == 1)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1522910), Genre = Context.Genres.First(g => g.Id == 3)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1522910), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1727014), Genre = Context.Genres.First(g => g.Id == 7)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1727014), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1727014), Genre = Context.Genres.First(g => g.Id == 18)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1613429), Genre = Context.Genres.First(g => g.Id == 5)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1613429), Genre = Context.Genres.First(g => g.Id == 8)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1253249), Genre = Context.Genres.First(g => g.Id == 4)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1253249), Genre = Context.Genres.First(g => g.Id == 6)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1253249), Genre = Context.Genres.First(g => g.Id == 10)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1252161), Genre = Context.Genres.First(g => g.Id == 4)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1252161), Genre = Context.Genres.First(g => g.Id == 6)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1252161), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 2325522), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 2325522), Genre = Context.Genres.First(g => g.Id == 14)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 2325522), Genre = Context.Genres.First(g => g.Id == 26)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1342415), Genre = Context.Genres.First(g => g.Id == 1)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1342415), Genre = Context.Genres.First(g => g.Id == 9)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1342415), Genre = Context.Genres.First(g => g.Id == 14)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1506162), Genre = Context.Genres.First(g => g.Id == 8)},
                new TitleGenre { Title = Context.Titles.First(t => t.Id == 1506162), Genre = Context.Genres.First(g => g.Id == 10)}
            };
        }
    }
}
