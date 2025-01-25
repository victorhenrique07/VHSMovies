using Castle.Core.Logging;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Headers;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Handlers;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;
using FluentAssertions;

namespace VHSMovies.Test
{
    public class FileReadersTest
    {
        private readonly Mock<ITitleRepository<Title>> titleRepositoryMock;
        private readonly Mock<ITitleGenreRepository> titleGenreRepositoryMock;
        private readonly Mock<IPersonRepository> peopleRepositoryMock;
        private readonly Mock<ICastRepository> castRepositoryMock;
        private readonly Mock<ILogger<ReadMoviesCommandHandler>> loggerTitlesMock;
        private readonly Mock<ILogger<ReadPeopleCommandHandler>> loggerPeopleMock;
        private readonly Mock<ILogger<ReadCastCommandHandler>> loggerCastMock;
        private readonly Mock<ILogger<ReadTitlesGenresCommandHandler>> loggerGenreMock;

        public FileReadersTest()
        {
            this.titleRepositoryMock = new();
            this.titleGenreRepositoryMock = new();
            this.peopleRepositoryMock = new();
            this.castRepositoryMock = new();
            this.loggerTitlesMock = new();
            this.loggerPeopleMock = new();
            this.loggerCastMock = new();
            this.loggerGenreMock = new();
        }

        private List<Dictionary<string, string>> invalidRows = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string>
            {
                {"id", "2"},
                {"name", "TitleTest" },
                {"description", "OverviewTest" },
                {"imdbId", "10" },
                {"duration", "120" }
            },
            new Dictionary<string, string>
            {
                {"id", "3"},
                {"name", "TitleTest2" },
                {"description", "OverviewTest2" },
                {"imdbId", "12" },
                {"duration", "120" }
            }
        };

        [Fact]
        public async Task Read_Titles_With_Valid_Rows()
        {
            // Arrange
            var command = new ReadMoviesCommand()
            {
                TitlesRows = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        {"filmid", "1"},
                        {"title", "TitleTest1" },
                        {"backdrop_path", "backdrop_one" },
                        {"overview", "OverviewTest1" },
                        {"poster_path", "poster_one" },
                        {"imdb_id", "11" },
                        {"runtime", "120" },
                        {"vote_average", "9" },
                        {"vote_count", "10" },
                    },
                    new Dictionary<string, string>()
                    {
                        {"filmid", "2"},
                        {"title", "TitleTest2" },
                        {"backdrop_path", "backdrop_one" },
                        {"overview", "OverviewTest2" },
                        {"poster_path", "poster_one" },
                        {"imdb_id", "12" },
                        {"runtime", "120" },
                        {"vote_average", "8" },
                        {"vote_count", "1000" },
                    },
                    new Dictionary<string, string>()
                    {
                        {"filmid", "3"},
                        {"title", "TitleTest3" },
                        {"backdrop_path", "backdrop_one" },
                        {"overview", "OverviewTest3" },
                        {"imdb_id", "13" },
                        {"poster_path", "poster_one" },
                        {"runtime", "120" },
                        {"vote_average", "9.2" },
                        {"vote_count", "1290" },
                    },
                }
            };

            var handler = new ReadMoviesCommandHandler(titleRepositoryMock.Object, loggerTitlesMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Read_Titles_With_Invalid_Rows_Should_Throw_Exception()
        {
            var command = new ReadMoviesCommand()
            {
                TitlesRows = invalidRows
            };

            var handler = new ReadMoviesCommandHandler(titleRepositoryMock.Object, loggerTitlesMock.Object);

            // Act
            Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await result.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cabeçalhos não correspondentes.");
        }

        [Fact]
        public async Task Read_People_With_Valid_Rows()
        {
            // Arrange
            var command = new ReadPeopleCommand()
            {
                PeopleRows = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        {"personid", "1"},
                        {"name_", "TestName1"}
                    }
                }
            };

            var handler = new ReadPeopleCommandHandler(peopleRepositoryMock.Object, loggerPeopleMock.Object, titleRepositoryMock.Object, castRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Read_People_With_Invalid_Rows_Should_Throw_Exception()
        {
            // Arrange
            var command = new ReadPeopleCommand()
            {
                PeopleRows = invalidRows
            };

            var handler = new ReadPeopleCommandHandler(peopleRepositoryMock.Object, loggerPeopleMock.Object, titleRepositoryMock.Object, castRepositoryMock.Object);

            // Act
            Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await result.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cabeçalhos não correspondentes.");
        }

        [Fact]
        public async Task Read_Cast_With_Valid_Rows()
        {
            // Arrange
            var command = new ReadCastCommand()
            {
                CastRows = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        {"personid", "1"},
                        {"name", "TestName1"},
                        {"filmid", "1" },
                        {"departmentid", "1" }
                    }
                }
            };

            var handler = new ReadCastCommandHandler(peopleRepositoryMock.Object, loggerCastMock.Object, titleRepositoryMock.Object, castRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Read_Cast_With_Invalid_Rows_Should_Throw_Exception()
        {
            // Arrange
            var command = new ReadCastCommand()
            {
                CastRows = invalidRows
            };

            var handler = new ReadCastCommandHandler(peopleRepositoryMock.Object, loggerCastMock.Object, titleRepositoryMock.Object, castRepositoryMock.Object);

            // Act
            Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await result.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cabeçalhos não correspondentes.");
        }

        [Fact]
        public async Task Read_Genres_With_Valid_Rows()
        {
            // Arrange
            var command = new ReadTitlesGenresCommand()
            {
                GenresRows = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        {"filmid", "1"},
                        {"genreid", "1"}
                    }
                }
            };

            var handler = new ReadTitlesGenresCommandHandler(loggerGenreMock.Object, titleGenreRepositoryMock.Object, titleRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Read_Genres_With_Invalid_Rows_Should_Throw_Exception()
        {
            // Arrange
            var command = new ReadTitlesGenresCommand()
            {
                GenresRows = invalidRows
            };

            var handler = new ReadTitlesGenresCommandHandler(loggerGenreMock.Object, titleGenreRepositoryMock.Object, titleRepositoryMock.Object);

            // Act
            Func<Task> result = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await result.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Cabeçalhos não correspondentes.");
        }
    }
}
