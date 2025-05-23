using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Infrastructure
{
    [Collection("DatabaseCollection")]
    public class TitleRepositoryTests
    {
        private readonly DatabaseSetupFixture databaseFixture;

        public TitleRepositoryTests(DatabaseSetupFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
        }

        [Fact]
        public async Task ShouldSaveNewSingleTitle()
        {
            // Arrange
            using var context = databaseFixture.CreateInMemoryDbContext();

            Title title = new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161")
            {
                Id = 1297469,
                Relevance = 11.13m
            };

            TitleRepository repository = new TitleRepository(context);

            // Act
            await repository.RegisterAsync(title);
            await repository.SaveChangesAsync();

            var result = await repository.GetByIdAsync(title.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(title);
        }

        [Fact]
        public async Task ShouldSaveTitlesList()
        {
            // Arrange
            using var context = databaseFixture.CreateInMemoryDbContext();

            List<Title> titles = new List<Title>(){
                new Title("The Shawshank Redemption", TitleType.Movie, 1994, "tt0111161")
                {
                    Id = 1297469,
                    Relevance = 11.13m
                },
                new Title("The Dark Knight", TitleType.Movie, 2008, "tt0468569")
                {
                    Id = 1489427,
                    Relevance = 10.98m
                }
            };

            TitleRepository repository = new TitleRepository(context);

            // Act
            await repository.RegisterListAsync(titles);
            await repository.SaveChangesAsync();

            var result = await repository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(titles);
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ShouldReturnAllTitlesWithGenresAndRatings()
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            TitleRepository repository = new TitleRepository(context);

            // Act
            List<Title> titles = await repository.GetAll();

            // Assert
            titles.Should().NotBeNullOrEmpty();
            titles.Should().HaveCount(12);
            titles.Should().AllSatisfy(title =>
            {
                title.Genres.Should().NotBeNullOrEmpty();
                title.AverageRating.Should().BeGreaterThan(0);
                title.Name.Should().NotBeNullOrEmpty();
                title.Ratings.Should().NotBeNullOrEmpty();
                title.Ratings.Should().HaveCount(2);
            });
        }

        [Theory]
        [InlineData("IMDb")]
        [InlineData("Metacritic")]
        public async Task ShouldReturnAllByReviewerName(string reviewerName)
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            TitleRepository repository = new TitleRepository(context);

            // Act
            List<Title> titles = await repository.GetAllByReviewerName(reviewerName);

            // Assert
            titles.Should().NotBeNullOrEmpty();
            titles.Should().HaveCount(12);
            titles.Should().AllSatisfy(title =>
            {
                title.Genres.Should().NotBeNullOrEmpty();
                title.AverageRating.Should().BeGreaterThan(0);
                title.Name.Should().NotBeNullOrEmpty();
                title.Ratings.Should().NotBeNullOrEmpty();
                title.Ratings.Should().HaveCount(2);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(7)]
        public async Task ShouldReturnAllByGenreId(int genreId)
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            TitleRepository repository = new TitleRepository(context);

            // Act
            List<Title> titles = await repository.GetAllByGenreId(genreId);

            titles.Should().NotBeNullOrEmpty();
            titles.Should().AllSatisfy(title =>
            {
                title.Genres.Should().NotBeNullOrEmpty();
                title.Genres.Should().Contain(genre => genre.Genre.Id == genreId);
            });
        }

        [Theory]
        [InlineData(1297469, "The Shawshank Redemption")]
        [InlineData(1489427, "The Dark Knight")]
        [InlineData(1260356, "The Godfather")]
        [InlineData(1519096, "Breaking Bad")]
        [InlineData(1522910, "Game of Thrones")]
        [InlineData(1727014, "Sherlock")]
        public async Task ShouldReturnTitleById(int titleId, string titleExpectedName)
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            TitleRepository repository = new TitleRepository(context);

            // Act
            Title title = await repository.GetByIdAsync(titleId);

            // Assert
            title.Id.Should().Be(titleId);
            title.Name.Should().Be(titleExpectedName);
            title.Genres.Should().NotBeNullOrEmpty();
            title.Ratings.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("tt0111161", "The Shawshank Redemption", "IMDb")]
        [InlineData("tt0468569", "The Dark Knight", "IMDb")]
        [InlineData("tt0068646", "The Godfather", "IMDb")]
        [InlineData("tt0903747", "Breaking Bad", "IMDb")]
        [InlineData("tt0944947", "Game of Thrones", "IMDb")]
        [InlineData("tt1475582", "Sherlock", "IMDb")]
        [InlineData("mt0111161", "The Shawshank Redemption", "Metacritic")]
        [InlineData("mt0468569", "The Dark Knight", "Metacritic")]
        [InlineData("mt0068646", "The Godfather", "Metacritic")]
        [InlineData("mt0903747", "Breaking Bad", "Metacritic")]
        [InlineData("mt0944947", "Game of Thrones", "Metacritic")]
        [InlineData("mt1475582", "Sherlock", "Metacritic")]
        public async Task ShouldReturnTitleByExternalId(string titleExternalId, string titleExpectedName, string reviewerName)
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            TitleRepository repository = new TitleRepository(context);

            // Act
            Title title = await repository.GetByExternalIdAsync(titleExternalId);

            // Assert
            title.Ratings.Should().NotBeNullOrEmpty();
            title.Name.Should().Be(titleExpectedName);
            title.Genres.Should().NotBeNullOrEmpty();
            title.Ratings.FirstOrDefault(r => r.Reviewer == reviewerName)?
                .TitleExternalId
                .Should()
                .Be(titleExternalId);
        }
    }
}
