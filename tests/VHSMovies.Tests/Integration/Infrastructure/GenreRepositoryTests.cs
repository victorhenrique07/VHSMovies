using System;
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
    public class GenreRepositoryTests
    {
        private readonly DatabaseSetupFixture _fixture;

        public GenreRepositoryTests(DatabaseSetupFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldReturnAllGenres()
        {
            // Arrange
            using var context = _fixture.CreateInMemoryDbContext();
            _fixture.SeedDatabase(context);

            string[] genresList = _fixture.GetGenresList().Select(genre => genre.Name).ToArray();
            IReadOnlyCollection<Genre> genres = new List<Genre>();
            var repository = new GenreRepository(context);

            // Act
            genres = await repository.GetAll();

            // Assert
            genres.Should().NotBeEmpty();
            genres.Should().HaveCount(26);
            genres.Should().AllSatisfy(g =>
            {
                g.Id.Should().NotBe(0);
                g.Name.Should().BeOneOf(genresList);
            });
        }
    }
}
