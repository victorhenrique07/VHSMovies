using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure;
using VHSMovies.Infraestructure.Repository;
using AutoFixture;

namespace VHSMovies.Test
{
    public class RepositoriesTest
    {
        private TitleRepository<Title> titleRepository;
        private TitleRepository<Movie> movieRepository;
        private TitleGenreRepository titleGenreRepository;
        private PersonRepository personRepository;
        private CastRepository castRepository;

        private readonly Title TitleOne;
        private readonly Title TitleTwo;
        private readonly Movie MovieOne;
        private readonly Movie MovieTwo;
        private readonly Person PersonOne;
        private readonly Person PersonTwo;
        private readonly Cast CastOne;
        private readonly Cast CastTwo;
        private readonly List<TitleGenre> TitleGenres;

        public RepositoriesTest()
        {
            var options = new DbContextOptionsBuilder<DbContextClass>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var configurationMock = new ConfigurationBuilder().Build();

            var dbContext = new DbContextClass(configurationMock, options);

            titleRepository = new TitleRepository<Title>(dbContext);
            movieRepository = new TitleRepository<Movie>(dbContext);
            titleGenreRepository = new TitleGenreRepository(dbContext);
            personRepository = new PersonRepository(dbContext);
            castRepository = new CastRepository(dbContext);

            var fixture = new Fixture();

            fixture.Customize<Title>(composer =>
                composer.Without(t => t.Ratings));

            fixture.Customize<Movie>(composer =>
                composer.Without(t => t.Ratings));

            fixture.Customize<Person>(composer =>
                composer.Without(t => t.Titles));

            this.TitleOne = fixture.Create<Title>();
            this.TitleTwo = fixture.Create<Title>();
            this.MovieOne = fixture.Create<Movie>();
            this.MovieTwo = fixture.Create<Movie>();
            this.PersonOne = fixture.Create<Person>();
            this.PersonTwo = fixture.Create<Person>();

            this.CastOne = new Cast()
            {
                Id = 1,
                TitleId = this.TitleOne.Id,
                Title = this.TitleOne,
                PersonId = this.PersonOne.Id,
                Role = PersonRole.Actor
            };

            this.CastTwo = new Cast()
            {
                Id = 2,
                TitleId = this.TitleOne.Id,
                Title = this.TitleOne,
                PersonId = this.PersonTwo.Id,
                Role = PersonRole.Actor
            };

            this.TitleGenres = CreateTitlesGenresList();
        }

        private List<TitleGenre> CreateTitlesGenresList()
        {
            Random rnd = new();

            List<TitleGenre> titleGenres = new List<TitleGenre>();

            int[] genres = 
            {
                28,12,16
            };

            foreach (int i in genres)
            {
                TitleGenre genre = new TitleGenre()
                {
                    Title = this.TitleOne,
                    TitleId = this.TitleOne.Id,
                    GenreId = i
                };

                titleGenres.Add(genre);
            }

            foreach (int i in genres)
            {
                TitleGenre genre = new TitleGenre()
                {
                    Title = this.TitleTwo,
                    TitleId = this.TitleTwo.Id,
                    GenreId = i
                };

                titleGenres.Add(genre);
            }

            return titleGenres;
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Titles()
        {

            List<Title> list = new List<Title>()
            {
                TitleOne, TitleTwo
            };

            //Act
            await titleRepository.RegisterListAsync(list);
            IEnumerable<Title> allTitles = await titleRepository.GetAll();

            // Assert
            allTitles.Should().NotBeNull();
            allTitles.Should().BeEquivalentTo(list);
            allTitles.First().Should().BeEquivalentTo(list.First());
            allTitles.First().Should().Match<Title>(t =>
                t.Name == TitleOne.Name &&
                t.Id == TitleOne.Id &&
                t.Description == TitleOne.Description);

            allTitles.Last().Should().BeEquivalentTo(list.Last());
            allTitles.Last().Should().Match<Title>(t =>
                t.Name == TitleTwo.Name &&
                t.Id == TitleTwo.Id &&
                t.Description == TitleTwo.Description);
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Titles_As_Movies()
        {
            // Arrange
            List<Title> list = new List<Title>()
            {
                MovieOne, MovieTwo
            };

            //Act
            await titleRepository.RegisterListAsync(list);
            IEnumerable<Movie> allMovies = await movieRepository.GetAll();

            // Assert

            allMovies.Should().NotBeNull();
            allMovies.Should().BeEquivalentTo(list);
            allMovies.First().Should().Match<Movie>(t =>
                t.Name == MovieOne.Name &&
                t.Id == MovieOne.Id &&
                t.Description == MovieOne.Description);

            allMovies.Last().Should().Match<Movie>(t =>
                t.Name == MovieTwo.Name &&
                t.Id == MovieTwo.Id &&
                t.Description == MovieTwo.Description);
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_People()
        {
            // Arrange
            List<Person> people = new List<Person>()
            {
                PersonOne,
                PersonTwo,
            };

            // Act
            await personRepository.RegisterListAsync(people);
            IEnumerable<Person> allPeople = await personRepository.GetAll();

            // Assert
            allPeople.Should().NotBeEmpty();
            allPeople.Should().BeEquivalentTo(people);

            allPeople.First().Should().Match<Person>(p =>
                p.Id == PersonOne.Id &&
                p.Name == PersonOne.Name);
            allPeople.Last().Should().Match<Person>(p =>
                p.Id == PersonTwo.Id &&
                p.Name == PersonTwo.Name);
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Casts()
        {
            // Arrange
            List<Cast> casts = new List<Cast>()
            {
                CastOne,
                CastTwo,
            };

            // Act
            await castRepository.RegisterListAsync(casts);
            IEnumerable<Cast> allCasts = await castRepository.GetAll();

            // Assert
            allCasts.Should().NotBeEmpty();
            allCasts.Count().Should().Be(2);

            allCasts.First().Should().Match<Cast>(t =>
                t.PersonId == PersonOne.Id &&
                t.Role == PersonRole.Actor &&
                t.Title.Name == TitleOne.Name &&
                t.Title.Description == TitleOne.Description);


            allCasts.Last().Should().Match<Cast>(t =>
                t.PersonId == PersonTwo.Id &&
                t.Role == PersonRole.Actor &&
                t.Title.Name == TitleOne.Name &&
                t.Title.Description == TitleOne.Description);
        }

        [Fact]
        public async Task Test_If_Is_Saving_All_Titles_Genres()
        {
            // Arrange
            int[] genres =
            {
                28,12,16
            };

            // Act
            await titleGenreRepository.RegisterGenresList(TitleGenres);

            IEnumerable<TitleGenre> allTitleOneGenres = await titleGenreRepository.GetTitleGenresById(TitleOne.Id);
            IEnumerable<TitleGenre> allTitleTwoGenres = await titleGenreRepository.GetTitleGenresById(TitleTwo.Id);

            // Assert
            allTitleOneGenres.Should().HaveCount(3);
            allTitleTwoGenres.Should().HaveCount(3);

            foreach (TitleGenre item in allTitleOneGenres)
            {
                item.TitleId.Should().Be(TitleOne.Id);
                item.Title.Name.Should().Be(TitleOne.Name);
                item.Title.Description.Should().Be(TitleOne.Description);
                item.Title.Ratings.Should().BeEquivalentTo(TitleOne.Ratings);
                Assert.Contains(item.GenreId, genres);
            }

            foreach (TitleGenre item in allTitleTwoGenres)
            {
                item.TitleId.Should().Be(TitleTwo.Id);
                item.Title.Name.Should().Be(TitleTwo.Name);
                item.Title.Description.Should().Be(TitleTwo.Description);
                item.Title.Ratings.Should().BeEquivalentTo(TitleTwo.Ratings);
                Assert.Contains(item.GenreId, genres);
            }
        }
    }
}
