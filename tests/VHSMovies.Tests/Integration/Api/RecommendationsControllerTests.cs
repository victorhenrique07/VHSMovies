using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MySqlX.XDevAPI;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VHSMovies.Api.Integration.Main.Responses;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Api
{
    [Collection("DatabaseCollection")]
    public class RecommendationsControllerTests : IClassFixture<RedisSetupFixture>
    {
        private readonly IConnectionMultiplexer redis;
        private readonly DatabaseSetupFixture databaseFixture;
        private readonly HttpClient httpClient;

        public RecommendationsControllerTests(RedisSetupFixture redisFixture, DatabaseSetupFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;

            this.httpClient = redisFixture.CreateClient();

            var scope = redisFixture.Services.CreateScope();
            this.redis = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
        }

        [Fact]
        public async Task GetRecommendedTitlesReturnsExpectedData()
        {
            // Arrange
            using var context = databaseFixture.CreateInMemoryDbContext();
            databaseFixture.SeedDatabase(context);

            var redisDatabase = redis.GetDatabase();

            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints[0]);

            // Act
            var result = await httpClient.GetAsync("/api/titles/recommend?minimumRating=8");

            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            List<TitleResponse> titles = JsonSerializer.Deserialize<List<TitleResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            titles.Should().NotBeNullOrEmpty();
            titles.Should().HaveCount(10);
            titles.Should().AllSatisfy(title =>
            {
                title.Should().NotBeNull();
                title.Id.Should().NotBe(0);
                title.Name.Should().NotBeNullOrEmpty();
                title.ReleaseDate.Should().NotBeNull();
                title.AverageRating.Should().BeGreaterThanOrEqualTo(8);
                title.Genres.Should().NotBeNullOrEmpty();
                title.Description.Should().NotBeNullOrEmpty();
                title.PosterImageUrl.Should().NotBeNullOrEmpty();
                title.BackdropImageUrl.Should().NotBeNullOrEmpty();
            });
            server.Keys().Should().NotBeEmpty();
            server.Keys().Should().HaveCount(1);
        }

        [Theory]
        [InlineData(1297469, "The Shawshank Redemption")]
        [InlineData(1489427, "The Dark Knight")]
        [InlineData(1260356, "The Godfather")]
        [InlineData(1519096, "Breaking Bad")]
        public async Task GetTitleByIdShouldReturnExpectedTitle(int titleId, string expectedTitleName)
        {
            // Arrange
            using var context = databaseFixture.CreateInMemoryDbContext();
            databaseFixture.SeedDatabase(context);

            // Act
            var result = await httpClient.GetAsync($"/api/titles/{titleId}");

            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();

            TitleResponse title = JsonSerializer.Deserialize<TitleResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            title.Should().NotBeNull();
            title.Id.Should().Be(titleId);
            title.Name.Should().Be(expectedTitleName);
            title.ReleaseDate.Should().NotBeNull();
            title.Genres.Should().NotBeNullOrEmpty();
            title.Description.Should().NotBeNullOrEmpty();
            title.PosterImageUrl.Should().NotBeNullOrEmpty();
            title.BackdropImageUrl.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetTitleByIdShouldThrowNotFoundException()
        {
            // Arrange
            using var context = databaseFixture.CreateInMemoryDbContext();
            databaseFixture.SeedDatabase(context);

            // Act
            var result = await httpClient.GetAsync("/api/titles/1");

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            var content = await result.Content.ReadAsStringAsync();
            content.Should().Contain("Title not found");
        }
    }
}
