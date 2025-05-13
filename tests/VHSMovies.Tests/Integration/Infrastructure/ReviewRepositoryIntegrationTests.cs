using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Infrastructure
{
    [Collection("DatabaseCollection")]
    public class ReviewRepositoryIntegrationTests
    {
        private readonly DatabaseSetupFixture _fixture;

        public ReviewRepositoryIntegrationTests(DatabaseSetupFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldReturnAllReviews()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            IReadOnlyCollection<Review> reviews = new List<Review>();
            var repository = new ReviewRepository(context);

            // Act
            reviews = await repository.GetAll();

            // Assert
            reviews.Should().NotBeEmpty();
            reviews.Should().HaveCount(24);
        }

        [Theory]
        [InlineData("IMDb")]
        [InlineData("Metacritic")]
        public async Task ShoulReturnReviewByReviewerName(string reviewerName)
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            var repository = new ReviewRepository(context);

            // Act
            IReadOnlyCollection<Review> reviews = await repository.GetByReviewerName(reviewerName);

            // Assert
            reviews.Should().NotBeNull();
            reviews.Should().HaveCount(12);
            reviews.Should().AllSatisfy(review =>
            {
                review.Reviewer.Should().Be(reviewerName);
                review.TitleExternalId.Should().NotBeNullOrEmpty();
                review.Title.Should().NotBeNull();
            });
        }

        [Theory]
        [InlineData("tt0111161", "The Shawshank Redemption", "IMDb", 9.3, 2700000)]
        [InlineData("mt0111161", "The Shawshank Redemption", "Metacritic", 80.0, 80000)]
        [InlineData("tt0468569", "The Dark Knight", "IMDb", 9.0, 2600000)]
        [InlineData("mt0468569", "The Dark Knight", "Metacritic", 84.0, 85000)]
        [InlineData("tt0068646", "The Godfather", "IMDb", 9.2, 2000000)]
        [InlineData("mt0068646", "The Godfather", "Metacritic", 100.0, 75000)]
        [InlineData("tt0903747", "Breaking Bad", "IMDb", 9.5, 1900000)]
        [InlineData("mt0903747", "Breaking Bad", "Metacritic", 87.0, 60000)]
        [InlineData("tt0944947", "Game of Thrones", "IMDb", 9.2, 1900000)]
        [InlineData("mt0944947", "Game of Thrones", "Metacritic", 86.0, 65000)]
        public async Task ShouldReturnReviewByTitleExternalId(
            string titleExternalId, string expectedTitleName, string expectedReviewerName,
            decimal expectedRating, int expectedTotalReviews)
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            var repository = new ReviewRepository(context);

            // Act
            Review review = await repository.GetByTitleExternalId(titleExternalId);

            // Assert
            review.Should().NotBeNull();
            review.Title.Should().NotBeNull();
            review.TitleExternalId.Should().Be(titleExternalId);
            review.Title.Name.Should().Be(expectedTitleName);
            review.Reviewer.Should().Be(expectedReviewerName);
            review.Rating.Should().Be(expectedRating);
            review.ReviewCount.Should().Be(expectedTotalReviews);
        }

        [Fact]
        public async Task ShouldSaveReviewsList()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();

            var reviewRepository = new ReviewRepository(context);
            var titleRepository = new TitleRepository(context);

            var shawshank = new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161");
            var darkKnight = new Title("The Dark Knight", TitleType.Movie, 2008, "tt0468569");

            await titleRepository.RegisterListAsync(new List<Title> { shawshank, darkKnight });
            await titleRepository.SaveChangesAsync();

            var reviews = new List<Review>
            {
                new Review("IMDb", 9.3m, 2700000)
                {
                    TitleExternalId = "tt0111161",
                    Title = shawshank
                },
                new Review("Metacritic", 80.0m, 80000)
                {
                    TitleExternalId = "mt0111161",
                    Title = shawshank
                },
                new Review("IMDb", 9.2m, 2000000)
                {
                    TitleExternalId = "tt0468569",
                    Title = darkKnight
                },
                new Review("Metacritic", 84.0m, 85000)
                {
                    TitleExternalId = "mt0468569",
                    Title = darkKnight
                },
            };

            // Act
            await reviewRepository.AddReviews(reviews);
            await reviewRepository.SaveChangesAsync();

            var savedReviews = await reviewRepository.GetAll();

            // Assert
            savedReviews.Should().HaveCount(4);

            savedReviews.Should().ContainEquivalentOf(new
            {
                Source = "IMDb",
                Score = 9.3m,
                VoteCount = 2700000,
                TitleExternalId = "tt0111161",
                Title = new { Name = "The Shawshank Redemption" }
            }, options => options.ExcludingMissingMembers());

            savedReviews.Should().ContainEquivalentOf(new
            {
                Source = "Metacritic",
                Score = 80.0m,
                VoteCount = 80000,
                TitleExternalId = "mt0111161",
                Title = new { Name = "The Shawshank Redemption" }
            }, options => options.ExcludingMissingMembers());

            savedReviews.Should().ContainEquivalentOf(new
            {
                Source = "IMDb",
                Score = 9.2m,
                VoteCount = 2000000,
                TitleExternalId = "tt0468569",
                Title = new { Name = "The Dark Knight" }
            }, options => options.ExcludingMissingMembers());

            savedReviews.Should().ContainEquivalentOf(new
            {
                Source = "Metacritic",
                Score = 84.0m,
                VoteCount = 85000,
                TitleExternalId = "mt0468569",
                Title = new { Name = "The Dark Knight" }
            }, options => options.ExcludingMissingMembers());
        }


    }
}
