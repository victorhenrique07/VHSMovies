using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class GetRecommendedTitlesQueryHandler : IRequestHandler<GetRecommendedTitlesQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly IRecommendedTitlesRepository recomendedTitlesRepository;
        private readonly ICastRepository castRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<GetRecommendedTitlesQueryHandler> logger;

        public GetRecommendedTitlesQueryHandler(
            IRecommendedTitlesRepository recomendedTitlesRepository,
            ICastRepository castRepository,
            IGenreRepository genreRepository)
        {
            this.recomendedTitlesRepository = recomendedTitlesRepository;
            this.castRepository = castRepository;
            this.genreRepository = genreRepository;
        }

        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetRecommendedTitlesQuery query, CancellationToken cancellationToken)
        {
            IQueryable<RecommendedTitle> titles = recomendedTitlesRepository.Query();

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            IReadOnlyCollection<Genre> allGenres = await genreRepository.GetAll();

            IReadOnlyCollection<TitleResponse> response = new List<TitleResponse>();

            if (query.IncludeGenres != null || query.ExcludeGenres != null || query.MustInclude != null)
            {
                var includeGenres = new List<string>();
                var excludeGenres = new List<string>();
                var mustIncludeGenres = new List<string>();

                foreach (var genre in allGenres)
                {
                    if (query.IncludeGenres != null && query.IncludeGenres.Contains(genre.Id))
                        includeGenres.Add(genre.Name);

                    if (query.ExcludeGenres != null && query.ExcludeGenres.Contains(genre.Id))
                        excludeGenres.Add(genre.Name);

                    if (query.MustInclude != null && query.MustInclude.Contains(genre.Id))
                        mustIncludeGenres.Add(genre.Name);
                }

                if (includeGenres.Count != 0)
                {
                    titles = titles.Where(title => title.Genres.Any(genre => includeGenres.Contains(genre)));
                }

                if (excludeGenres.Count != 0)
                {
                    titles = titles.Where(title =>
                        !title.Genres.Any(genre => excludeGenres.Contains(genre)));
                }

                if (mustIncludeGenres.Count != 0)
                {
                    titles = titles.Where(title => mustIncludeGenres.All(genre => title.Genres.Contains(genre)));
                }
            }

            if (query.Types != null)
            {
                titles = titles.Where(title => query.Types.Any(titleType => title.Type == (int)titleType));
            }

            if (query.Actors?.Any() == true || query.Directors?.Any() == true || query.Writers?.Any() == true)
            {
                var roles = new List<(PersonRole Role, IReadOnlyCollection<string>? Names)>
                {
                    (PersonRole.Actor, query.Actors),
                    (PersonRole.Director, query.Directors),
                    (PersonRole.Writer, query.Writers)
                };

                var castFilters = roles
                    .Where(r => r.Names?.Any() == true)
                    .ToList();

                if (castFilters.Any())
                {
                    IEnumerable<int> matchingTitleIds = Enumerable.Empty<int>();

                    foreach (var (role, names) in castFilters)
                    {
                        var castMatches = await castRepository.GetCastsByPersonRole(role);
                        var matchedTitles = castMatches.Where(c => names.Contains(c.Person.Name))
                                                       .Select(c => c.Title.Id)
                                                       .Distinct();

                        matchingTitleIds = matchingTitleIds.Any() ? matchingTitleIds.Intersect(matchedTitles) : matchedTitles;
                    }

                    titles = titles.Where(title => matchingTitleIds.Contains(title.Id));
                }
            }

            if (query.MinimumRating != null)
            {
                titles = titles.Where(t => t.AverageRating >= query.MinimumRating);
            }

            if (query.YearsRange != null)
            {
                query.YearsRange = BubbleSort(query.YearsRange, query.YearsRange.Length);

                titles = titles.Where(t =>
                    t.ReleaseDate.HasValue &&
                    t.ReleaseDate > query.YearsRange[0] &&
                    t.ReleaseDate < query.YearsRange[1]);
            }

            if (query.TitlesToExclude != null && query.TitlesToExclude.Any())
            {
                titles = titles.Where(t => !query.TitlesToExclude.Contains(t.Id));
            }

            IReadOnlyCollection<RecommendedTitle> data = titles
                .OrderByDescending(t => t.Relevance)
                .Take(query.TitlesAmount)
                .ToList();

            response = data
                .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t, allGenres)).ToList();

            return response;
        }

        private static int[] BubbleSort(int[] arr, int n)
        {
            int i, j, temp;
            bool swapped;
            for (i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true;
                    }
                }

                if (swapped == false)
                    break;
            }

            return arr;
        }
    }
}
