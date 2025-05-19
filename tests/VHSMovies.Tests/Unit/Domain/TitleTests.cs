using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Tests.Unit.Domain
{
    public class TitleTests
    {
        public TitleTests()
        {
        }

        private readonly Title darkKnightTitle = new Title("The Dark Knight", TitleType.Movie, 2008, "tt0468569")
        {
            Id = 1489427,
            Genres = new List<TitleGenre>()
            {
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 1,
                        Name = "Action"
                    }
                },
                new TitleGenre()
                {
                    Genre = new Genre()
                    {
                        Id = 9,
                        Name = "Drama"
                    }
                },
            },
            Ratings = new List<Review>
            {
                new Review("IMDb", 9.0m, 2600000)
                {
                    Id = 3,
                    TitleExternalId = "tt0468569"
                },
                new Review("Metacritic", 84.0m, 85000)
                {
                    Id = 4,
                    TitleExternalId = "mt0468569"
                }
            }
        };

        [Fact]
        public async Task ShouldCalculateAverageRatingLessThanOrEqualTo10()
        {
            var averageRating = darkKnightTitle.AverageRating;

            averageRating.Should().BeLessThanOrEqualTo(8.7m);
        }

        [Fact]
        public async Task ShouldCalculateTotalReviews()
        {
            darkKnightTitle.TotalReviews.Should().Be(2685000);
        }

        [Fact]
        public async Task ShouldCalculateRelevance()
        {
            darkKnightTitle.SetRelevance();

            darkKnightTitle.Relevance.Should().Be(10.779m);
        }
    }
}
