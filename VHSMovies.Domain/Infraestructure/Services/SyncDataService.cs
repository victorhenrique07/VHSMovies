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

        public async Task UpdateTitlesAsync(string reviewerName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
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

                    foreach (var title in titles)
                    {
                        Review titleValues = title.Reviews.Where(p => p.Reviewer.Name == reviewerName).FirstOrDefault();

                        var updatedTitle = ReadTitlePage(titleValues.TitleExternalUrl, titleValues.Reviewer.ShortName);

                        Review review = title.Reviews.Where(r => r.Reviewer.ShortName.Equals(reviewerName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                        var updatedRating = updatedTitle.Reviews.Where(r => r.Reviewer.ShortName.Equals(reviewerName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                        if (updatedRating != null)
                        {
                            review.Rating = updatedRating.Rating;
                        }

                        foreach (var castMember in updatedTitle.Cast)
                        {
                            if (castMember.Person != null)
                            {
                                var existingPerson = await personRepository.GetByExternalIdAsync(castMember.Person.ExternalId);

                                if (existingPerson == null)
                                {
                                    await personRepository.RegisterAsync(castMember.Person);
                                    existingPerson = castMember.Person;
                                }

                                castMember.Person = existingPerson;
                            }
                        }

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

                    if (updatedTVShows.Any())
                    {
                        await tvshowRepository.UpdateAsync(updatedTVShows);
                    }

                    if (updatedMovies.Any())
                    {
                        await movieRepository.UpdateAsync(updatedMovies);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao atualizar os títulos: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task RegisterCastAsync(ICastRepository castRepository, IPersonRepository personRepository, IEnumerable<Cast> castMembers)
        {
            var castList = castMembers.ToList();

            List<Cast> castsToRegister = new List<Cast>();

            foreach (var cast in castList)
            {
                var person = cast.Person;

                Person existingPerson = await personRepository.GetByExternalIdAsync(person.ExternalId);

                if (existingPerson == null)
                {
                    await personRepository.RegisterAsync(person);
                }

                var existingCast = await castRepository.GetCastForTitleAsync(cast.Title.Id, person.Id);

                if (existingCast == null)
                {
                    castsToRegister.Add(new Cast(person, cast.Title));
                }
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
