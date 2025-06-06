﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Tests.Integration.Setup;

namespace VHSMovies.Tests.Integration.Infrastructure
{
    [Collection("DatabaseCollection")]
    public class RecommendedTitleRepositoryTests
    {
        private readonly DatabaseSetupFixture databaseFixture;

        public RecommendedTitleRepositoryTests(DatabaseSetupFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
        }

        [Fact]
        public async Task ShouldReturnAllRecommendedTitles()
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            IReadOnlyCollection<RecommendedTitle> titles = new List<RecommendedTitle>();
            var repository = new RecommendedTitlesRepository(context);

            // Act
            titles = repository.GetAllRecommendedTitles().Result;

            // Assert
            titles.Should().NotBeNull();
            titles.Should().HaveCount(12);
        }

        [Theory]
        [InlineData(1297469, "The Shawshank Redemption")]
        [InlineData(1260356, "The Godfather")]
        [InlineData(1489427, "The Dark Knight")]
        [InlineData(1519096, "Breaking Bad")]
        [InlineData(1522910, "Game of Thrones")]
        public async Task ShouldReturnTitleById(int id, string name)
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            var repository = new RecommendedTitlesRepository(context);

            // Act
            var title = await repository.GetById(id);

            // Assert
            title.Should().NotBeNull();
            title.Id.Should().Be(id);
            title.Name.Should().Be(name);
        }

        [Fact]
        public async Task ShouldReturnAllRecommendedTitlesAsQueryable()
        {
            // Arrange
            PopulateDatabase seed = new PopulateDatabase();
            using var context = databaseFixture.CreateInMemoryDbContext();
            seed.SeedDatabase(context);

            IQueryable<RecommendedTitle> titles = new List<RecommendedTitle>().AsQueryable();
            var repository = new RecommendedTitlesRepository(context);

            // Act
            titles = repository.Query();

            // Assert
            titles.Should().NotBeNull();
            titles.Should().HaveCount(12);
        }
    }
}
