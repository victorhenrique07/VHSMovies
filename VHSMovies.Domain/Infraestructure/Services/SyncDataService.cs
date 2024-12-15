using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Domain.Infraestructure.Services
{
    public class SyncDataService
    {
        private readonly IEnumerable<IDataReader> dataReaders;
        private readonly IPersonRepository personRepository;
        private readonly ITitleRepository<Movie> movieRepository;
        private readonly ITitleRepository<TVShow> tvshowRepository;

        public SyncDataService(IPersonRepository personRepository, IEnumerable<IDataReader> dataReaders,
            ITitleRepository<Movie> movieRepository, ITitleRepository<TVShow> tvshowRepository)
        {
            this.dataReaders = dataReaders;
            this.personRepository = personRepository;
            this.movieRepository = movieRepository;
            this.tvshowRepository = tvshowRepository;
        }

        public void RegisterNewData(string reviewerName)
        {
            IReadOnlyCollection<Title> titlesList = ReadTitles(reviewerName);

            List<Movie> movies = movieRepository.GetAll(reviewerName).Result.ToList();
            List<TVShow> tvshows = tvshowRepository.GetAll(reviewerName).Result.ToList();

            List<Title> titles = new List<Title>();

            titles.AddRange(movies);
            titles.AddRange(tvshows);

            foreach (Title title in titlesList)
            {
                bool existingTitle = titles
                    .Select(p => p.ExternalId == title.ExternalId || p.Name == title.Name).Any();

                if (!existingTitle)
                {
                    List<Person> allPeople = title.Cast.Actors
                        .Concat(title.Cast.Directors)
                        .Concat(title.Cast.Writers).ToList();

                    foreach (Person person in allPeople)
                    {
                        Person existingPerson = personRepository.GetByExternalIdAsync(person.ExternalId).Result;

                        if (existingPerson != null)
                        {
                            RegisterPeople(person);
                        }
                    }

                    if (title is Movie movie)
                        movieRepository.RegisterAsync(movie);
                    if (title is TVShow tvshow)
                        tvshowRepository.RegisterAsync(tvshow);
                }
            }
        }

        public void UpdateTitles(string reviewerName)
        {
            List<Movie> movies = movieRepository.GetAll(reviewerName).Result.ToList();
            List<TVShow> tvshows = tvshowRepository.GetAll(reviewerName).Result.ToList();

            List<Title> titles = new List<Title>();

            titles.AddRange(movies);
            titles.AddRange(tvshows);

            foreach (Title title in titles)
            {
                Title updatedTitle = ReadTitlePage(title.Url, reviewerName);

                foreach (Review review in title.Ratings)
                {
                    if (!review.Reviewer.ToLower().Equals(reviewerName.ToLower()))
                        continue;

                    review.Rating = updatedTitle.Ratings.FirstOrDefault().Rating;
                }

                var allPeople = title.Cast.Actors
                    .Concat(title.Cast.Directors)
                    .Concat(title.Cast.Writers);

                foreach (Person person in allPeople)
                {
                    Person existingPerson = personRepository.GetByExternalIdAsync(person.ExternalId).Result;

                    if (existingPerson != null)
                    {
                        RegisterPeople(person);
                    }
                }

                title.Cast = updatedTitle.Cast;
                title.Genres = updatedTitle.Genres;

                if (title is TVShow tvshow && updatedTitle is TVShow updatedTVShow)
                {
                    tvshow.Seasons = updatedTVShow.Seasons;

                    tvshowRepository.UpdateAsync(tvshow);
                    continue;
                }
                else if (title is Movie movie)
                    movieRepository.UpdateAsync(movie);
            }
        }

        private IReadOnlyCollection<Title> ReadTitles(string sourceName)
        {
            IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);

            IReadOnlyCollection<Title> titles = dataReader.ReadTitles();

            return titles;
        }

        private Title ReadTitlePage(string url, string sourceName)
        {
            IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);

            Title titles = dataReader.ReadTitle(url);

            return titles;
        }

        public void RegisterPeople(Person person)
        {
            bool existingPerson = personRepository.GetByExternalIdAsync(person.ExternalId).Result != null ? true : false;

            if (!existingPerson)
            {
                personRepository.RegisterAsync(person);
            }

            throw new Exception($"Person with Name {person.Name} ExternalId {person.ExternalId} already exists.");
        }
    }
}
