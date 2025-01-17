using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.Test
{
    public class RepositoriesTest
    {
        private readonly Mock<ITitleRepository<Title>> titleRepository;
        private readonly Mock<ITitleRepository<Movie>> movieRepository;
        private readonly Mock<ITitleGenreRepository> titleGenreRepositoryMock;
        private readonly Mock<IPersonRepository> peopleRepositoryMock;
        private readonly Mock<ICastRepository> castRepositoryMock;

        public RepositoriesTest()
        {
            this.titleRepository = new();
            this.movieRepository = new();
            this.titleGenreRepositoryMock = new();
            this.peopleRepositoryMock = new();
            this.castRepositoryMock = new();
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Titles()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                Review review = new Review("IMDb", 9.0m);

                Title title = new Title("Title1", "description1", new List<Review>() { review })
                {
                    Id = 1
                };
                Title title2 = new Title("Title2", "description2", new List<Review>() { review })
                {
                    Id = 2
                };

                List<Title> list = new List<Title>()
                {
                    title, title2
                };

                titleRepository
                    .Setup(x => x.RegisterListAsync(list))
                    .Returns(Task.CompletedTask);

                titleRepository
                    .Setup(x => x.GetAll())
                    .ReturnsAsync(list);

                //Act
                await titleRepository.Object.RegisterListAsync(list);
                IEnumerable<Title> allTitles = await titleRepository.Object.GetAll();

                // Assert
                titleRepository.Verify(x => x.RegisterListAsync(It.Is<List<Title>>(l => l.Count == 2)), Times.Once);
                allTitles.Count().Should().Be(2);
            }
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Titles_As_Movies()
        {
            // Arrange
            Review review = new Review("IMDb", 9.0m);

            Movie title = new Movie("Title1", "description1", new List<Review>() { review }, 120)
            {
                Id = 1
            };
            Movie title2 = new Movie("Title2", "description2", new List<Review>() { review }, 120)
            {
                Id = 2
            };

            List<Title> list = new List<Title>()
            {
                title, title2
            };

            titleRepository
                .Setup(x => x.RegisterListAsync(list))
                .Returns(Task.CompletedTask);

            titleRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(list);

            movieRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(
                    new List<Movie>()
                    {
                        title, title2
                    }
                );

            //Act
            await titleRepository.Object.RegisterListAsync(list);
            IEnumerable<Title> allTitles = await titleRepository.Object.GetAll();
            IEnumerable<Movie> allMovies = await movieRepository.Object.GetAll();

            var firstTitle = allMovies.FirstOrDefault();
            var lastTitle = allTitles.Last();

            // Assert
            titleRepository.Verify(x => x.RegisterListAsync(It.Is<List<Title>>(l => l.Count == 2)), Times.Once);

            firstTitle.Should().NotBeNull();
            firstTitle.Should().Match<Title>(t =>
                t.Name == "Title1" &&
                t.Id == 1 &&
                t.Description == "description1" &&
                t.Ratings.Count == 1 &&
                t.Ratings.First().Reviewer == "IMDb" &&
                t.Ratings.First().Rating == 9.0m);

            lastTitle.Should().NotBeNull();
            lastTitle.Should().Match<Title>(t =>
                t.Name == "Title2" &&
                t.Id == 2 &&
                t.Description == "description2" &&
                t.Ratings.Count == 1 &&
                t.Ratings.First().Reviewer == "IMDb" &&
                t.Ratings.First().Rating == 9.0m);
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_People()
        {
            // Arrange
            Person person1 = new Person("Person1");

            Person person2 = new Person("Person2");

            List<Person> people = new List<Person>()
            {
                person1,
                person2,
            };

            // Act

            peopleRepositoryMock
                .Setup(x => x.RegisterListAsync(people))
                .Returns(Task.CompletedTask);

            peopleRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(people);

            await peopleRepositoryMock.Object.RegisterListAsync(people);
            IEnumerable<Person> allPeople = await peopleRepositoryMock.Object.GetAll();

            Person firstPerson = allPeople.First();
            Person secondPerson = allPeople.Last();

            // Assert
            peopleRepositoryMock.Verify(x => x.RegisterListAsync(It.Is<List<Person>>(l => l.Count == 2)), Times.Once);

            allPeople.Should().NotBeEmpty();
            allPeople.Count().Should().Be(2);

            firstPerson.Name.Should().Be("Person1");

            secondPerson.Name.Should().Be("Person2");
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Casts()
        {
            Review review = new Review("IMDb", 9.0m);

            Movie title = new Movie("Title1", "description1", new List<Review>() { review }, 120);

            Person person1 = new Person("Person1");

            Person person2 = new Person("Person2");

            Cast cast1 = new Cast(person1, title)
            {
                Role = PersonRole.Actor
            };

            person1.Titles.Add(cast1);

            Cast cast2 = new Cast(person2, title)
            {
                Role = PersonRole.Actor
            };

            person2.Titles.Add(cast2);

            List<Cast> casts = new List<Cast>()
            {
                cast1,
                cast2,
            };

            castRepositoryMock
                .Setup(x => x.RegisterListAsync(casts))
                .Returns(Task.CompletedTask);

            castRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(casts);

            await castRepositoryMock.Object.RegisterListAsync(casts);
            IEnumerable<Cast> allCasts = await castRepositoryMock.Object.GetAll();

            Cast firstCast = allCasts.First();
            Cast secondCast = allCasts.Last();

            allCasts.Should().NotBeEmpty();
            allCasts.Count().Should().Be(2);

            firstCast.Should().Match<Cast>(t =>
                t.Person.Name == "Person1" &&
                t.Role == PersonRole.Actor &&
                t.Title.Name == "Title1" &&
                t.Title.Description == "description1" &&
                t.Title.Ratings.Count == 1 &&
                t.Title.Ratings.First().Reviewer == "IMDb" &&
                t.Title.Ratings.First().Rating == 9.0m);


            secondCast.Should().Match<Cast>(t =>
                t.Person.Name == "Person2" &&
                t.Role == PersonRole.Actor &&
                t.Title.Name == "Title1" &&
                t.Title.Description == "description1" &&
                t.Title.Ratings.Count == 1 &&
                t.Title.Ratings.First().Reviewer == "IMDb" &&
                t.Title.Ratings.First().Rating == 9.0m);
        }
    }
}
