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
                var castRepository = scope.ServiceProvider.GetRequiredService<ICastRepository>();
                var dataReaders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDataReader>>();

                IReadOnlyCollection<Title> titlesList = ReadTitles(dataReaders, reviewerName);

                IEnumerable<Movie> movies = await movieRepository.GetAllByReviewerName(reviewerName);
                IEnumerable<TVShow> tvshows = await tvshowRepository.GetAllByReviewerName(reviewerName);

                List<Title> titles = new List<Title>();
                titles.AddRange(movies);
                titles.AddRange(tvshows);

                List<Movie> unregisteredMovies = new List<Movie>();
                List<TVShow> unregisteredTVShows = new List<TVShow>();

                var allPeople = new List<Person>();
                var castMembers = new List<Cast>();

                foreach (Title title in titlesList)
                {
                    bool existingTitle = titles.Any(p => p.ExternalId == title.ExternalId || p.Name == title.Name);

                    if (!existingTitle)
                    {
                        allPeople.AddRange(title.Cast.Select(p => p.Person));

                        // Adiciona os membros do cast para registrar as relações
                        castMembers.AddRange(title.Cast);

                        if (title is Movie movie)
                            unregisteredMovies.Add(movie);
                        if (title is TVShow tvshow)
                            unregisteredTVShows.Add(tvshow);
                    }
                }

                if (allPeople.Any())
                {
                    await RegisterPeople(personRepository, allPeople.ToList());
                    await RegisterCastAsync(castRepository, personRepository, castMembers);
                }

                await movieRepository.RegisterAsync(unregisteredMovies);
                await tvshowRepository.RegisterAsync(unregisteredTVShows);
            }

            await UpdateTitlesAsync(reviewerName);
        }

        public async Task UpdateTitlesAsync(string reviewerName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var personRepository = scope.ServiceProvider.GetRequiredService<IPersonRepository>();
                var movieRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<Movie>>();
                var tvshowRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<TVShow>>();
                var castRepository = scope.ServiceProvider.GetRequiredService<ICastRepository>();

                var movies = await movieRepository.GetAllByReviewerName(reviewerName);
                var tvShows = await tvshowRepository.GetAllByReviewerName(reviewerName);

                var titles = new List<Title>();
                titles.AddRange(movies);
                titles.AddRange(tvShows);

                var updatedMovies = new List<Movie>();
                var updatedTVShows = new List<TVShow>();

                var allPeople = new List<Person>();
                var castMembers = new List<Cast>();

                foreach (var title in titles)
                {
                    var updatedTitle = ReadTitlePage(title.Url, reviewerName);

                    foreach (var review in title.Ratings.Where(r => r.Reviewer.Equals(reviewerName, StringComparison.OrdinalIgnoreCase)))
                    {
                        review.Rating = updatedTitle.Ratings.FirstOrDefault().Rating;
                    }

                    foreach (var castMember in updatedTitle.Cast)
                    {
                        if (castMember.Person != null)
                            allPeople.Add(castMember.Person);

                        castMembers.Add(castMember);
                    }

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

                var distinctPeople = allPeople.ToList();
                var unregisteredPeople = new List<Person>();

                foreach (var person in distinctPeople)
                {
                    var existingPerson = await personRepository.GetByExternalIdAsync(person.ExternalId);
                    if (existingPerson == null)
                    {
                        unregisteredPeople.Add(person);
                    }
                }

                if (unregisteredPeople.Any())
                {
                    await RegisterPeople(personRepository, unregisteredPeople);
                }

                await RegisterCastAsync(castRepository, personRepository, castMembers);

                await tvshowRepository.UpdateAsync(updatedTVShows);
                await movieRepository.UpdateAsync(updatedMovies);
            }
        }

        public async Task RegisterCastAsync(ICastRepository castRepository, IPersonRepository personRepository, IEnumerable<Cast> castMembers)
        {
            var castList = castMembers.ToList();

            HashSet<Person> people = new HashSet<Person>();
            List<Cast> castsToRegister = new List<Cast>();

            foreach (var cast in castList)
            {
                var person = cast.Person;

                var existingPerson = await personRepository.GetByExternalIdAsync(person.ExternalId);

                if (existingPerson == null)
                {
                    people.Add(person);
                }

                var existingCast = await castRepository.GetCastForTitleAsync(cast.Title.Id, person.Id);

                if (existingCast == null)
                {
                    castsToRegister.Add(new Cast(person, cast.Title));
                }
            }

            if (people.Any())
            {
                await personRepository.RegisterAsync(people.ToList());
            }

            if (castsToRegister.Any())
            {
                await castRepository.UpdateAsync(castsToRegister);
            }
        }

        private IReadOnlyCollection<Title> ReadTitles(IEnumerable<IDataReader> dataReaders, string sourceName)
        {
            IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);
            return dataReader.ReadTitles();
        }

        private async Task RegisterPeople(IPersonRepository personRepository, List<Person> people)
        {
            var peopleToAdd = new List<Person>();

            foreach (Person personItem in people)
            {
                var existingPerson = await personRepository.GetByExternalIdAsync(personItem.ExternalId);

                if (existingPerson == null)
                {
                    peopleToAdd.Add(personItem);
                }
            }

            if (peopleToAdd.Any())
            {
                await personRepository.RegisterAsync(peopleToAdd);
            }
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
