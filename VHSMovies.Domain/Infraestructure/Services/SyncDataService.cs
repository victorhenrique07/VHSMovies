using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SyncDataService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async Task RegisterNewData(string reviewerName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var personRepository = scope.ServiceProvider.GetRequiredService<IPersonRepository>();
                var movieRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<Movie>>();
                var tvshowRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<TVShow>>();
                var dataReaders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDataReader>>();

                IReadOnlyCollection<Title> titlesList = ReadTitles(dataReaders, reviewerName);

                IEnumerable<Movie> movies = await movieRepository.GetAll(reviewerName).ConfigureAwait(false);
                IEnumerable<TVShow> tvshows = await tvshowRepository.GetAll(reviewerName).ConfigureAwait(false);

                List<Title> titles = new List<Title>();
                titles.AddRange(movies);
                titles.AddRange(tvshows);

                List<Movie> unregisteredMovies = new List<Movie>();
                List<TVShow> unregisteredTVShows = new List<TVShow>();

                foreach (Title title in titlesList)
                {
                    bool existingTitle = titles.Any(p => p.ExternalId == title.ExternalId || p.Name == title.Name);

                    if (!existingTitle)
                    {
                        if (title is Movie movie)
                            unregisteredMovies.Add(movie);
                        if (title is TVShow tvshow)
                            unregisteredTVShows.Add(tvshow);

                        List<Person> allPeople = title.Cast.Actors
                            .Concat(title.Cast.Directors)
                            .Concat(title.Cast.Writers).ToList();

                        RegisterPeople(personRepository, allPeople);
                    }
                }

                await movieRepository.RegisterAsync(unregisteredMovies);
                await tvshowRepository.RegisterAsync(unregisteredTVShows);
            }
        }

        public async Task UpdateTitlesAsync(string reviewerName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var personRepository = scope.ServiceProvider.GetRequiredService<IPersonRepository>();
                var movieRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<Movie>>();
                var tvshowRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<TVShow>>();

                var movies = await movieRepository.GetAll(reviewerName);
                var tvShows = await tvshowRepository.GetAll(reviewerName);

                var titles = new List<Title>();
                titles.AddRange(movies);
                titles.AddRange(tvShows);

                var updatedMovies = new List<Movie>();
                var updatedTVShows = new List<TVShow>();

                foreach (var title in titles)
                {
                    var updatedTitle = ReadTitlePage(title.Url, reviewerName);

                    foreach (var review in title.Ratings.Where(r => r.Reviewer.Equals(reviewerName, StringComparison.OrdinalIgnoreCase)))
                    {
                        review.Rating = updatedTitle.Ratings.FirstOrDefault().Rating;
                    }

                    var allPeople = title.Cast.Actors
                        .Concat(title.Cast.Directors)
                        .Concat(title.Cast.Writers)
                        .ToList();

                    RegisterPeople(personRepository, allPeople);

                    title.Cast = updatedTitle.Cast;
                    title.Genres = updatedTitle.Genres;

                    if (title is TVShow tvShow && updatedTitle is TVShow updatedTVShow)
                    {
                        tvShow.Seasons = updatedTVShow.Seasons;
                        updatedTVShows.Add(tvShow);
                    }
                    else if (title is Movie movie)
                    {
                        updatedMovies.Add(movie);
                    }
                }

                if (updatedMovies.Any())
                {
                    await movieRepository.UpdateAsync(updatedMovies);
                }

                if (updatedTVShows.Any())
                {
                    await tvshowRepository.UpdateAsync(updatedTVShows);
                }

                await movieRepository.SaveChanges();
            }
        }



        private IReadOnlyCollection<Title> ReadTitles(IEnumerable<IDataReader> dataReaders, string sourceName)
        {
            IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);
            return dataReader.ReadTitles();
        }

        private void RegisterPeople(IPersonRepository personRepository, List<Person> people)
        {
            foreach (Person personItem in people)
            {
                var existingPerson = personRepository.GetByExternalIdAsync(personItem.ExternalId).Result;

                if (existingPerson == null)
                    people.Remove(personItem);
            }

            personRepository.RegisterAsync(people);
        }

        private Title ReadTitlePage(string url, string sourceName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dataReaders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDataReader>>();

                IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);

                Title titles = dataReader.ReadTitle(url);

                return titles;
            }
        }
    }
}
