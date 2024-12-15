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

        public async Task RegisterNewData(string reviewerName)
        {
            IReadOnlyCollection<Title> titlesList = ReadTitles(reviewerName);

            List<Movie> movies = movieRepository.GetAll(reviewerName).Result.ToList();
            List<TVShow> tvshows = tvshowRepository.GetAll(reviewerName).Result.ToList();

            List<Title> titles = new List<Title>();

            titles.AddRange(movies);
            titles.AddRange(tvshows);

            List<Movie> unregisteredMovies = new List<Movie>();
            List<TVShow> unregisteredTVShows = new List<TVShow>();

            foreach (Title title in titlesList)
            {
                bool existingTitle = titles
                    .Select(p => p.ExternalId == title.ExternalId || p.Name == title.Name).Any();

                if (!existingTitle)
                {
                    if (title is Movie movie)
                        unregisteredMovies.Add(movie);
                    if (title is TVShow tvshow)
                        unregisteredTVShows.Add(tvshow);

                    List<Person> allPeople = title.Cast.Actors
                        .Concat(title.Cast.Directors)
                        .Concat(title.Cast.Writers).ToList();

                    RegisterPeople(allPeople);
                }
            }

            await movieRepository.RegisterAsync(unregisteredMovies);
           // await tvshowRepository.RegisterAsync(unregisteredTVShows);

           // await movieRepository.SaveChanges();
        }

        public void UpdateTitles(string reviewerName)
        {
            List<Movie> movies = movieRepository.GetAll(reviewerName).Result.ToList();
            List<TVShow> tvshows = tvshowRepository.GetAll(reviewerName).Result.ToList();

            List<Title> titles = new List<Title>();

            titles.AddRange(movies);
            titles.AddRange(tvshows);

            List<Movie> unregisteredMovies = new List<Movie>();
            List<TVShow> unregisteredTVShows = new List<TVShow>();

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

                RegisterPeople(allPeople.ToList());

                title.Cast = updatedTitle.Cast;
                title.Genres = updatedTitle.Genres;

                if (title is TVShow tvshow && updatedTitle is TVShow updatedTVShow)
                {
                    tvshow.Seasons = updatedTVShow.Seasons;

                    unregisteredTVShows.Add(tvshow);
                }
                else if (title is Movie movie)
                    unregisteredMovies.Add(movie);
                    
            }

            movieRepository.UpdateAsync(unregisteredMovies);
            tvshowRepository.UpdateAsync(unregisteredTVShows);

            movieRepository.SaveChanges();
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

        public void RegisterPeople(List<Person> people)
        {
            foreach (Person personItem in people)
            {
                Person existingPerson = personRepository.GetByExternalIdAsync(personItem.ExternalId).Result;

                if (existingPerson == null)
                    people.Remove(personItem);
            }

            personRepository.RegisterAsync(people);
        }
    }
}
