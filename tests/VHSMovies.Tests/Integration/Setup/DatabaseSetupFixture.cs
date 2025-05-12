using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure;

namespace VHSMovies.Tests.Integration.Setup
{
    public class DatabaseSetupFixture : IDisposable
    {
        public readonly DbContextClass Context;

        public DatabaseSetupFixture()
        {
            Context = DbContextHelper.CreateInMemoryDbContext();

            Context.RecommendedTitles.AddRangeAsync(GetRecommendedTitlesList());
            Context.Genres.AddRangeAsync(new List<Genre>
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
            });

            Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        private List<RecommendedTitle> GetRecommendedTitlesList()
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

                // Séries
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

                // Especiais
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

                // Minisséries
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
    }
}
