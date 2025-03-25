using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure.Repository;

namespace VHSMovies.Application.Handlers
{
    public class GetRecommendedTitlesQueryHandler : IRequestHandler<GetRecommendedTitlesQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly IRecommendedTitlesRepository recomendedTitlesRepository;
        private readonly ICastRepository castRepository;
        private readonly ITitleGenreRepository titleGenreRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<GetRecommendedTitlesQueryHandler> logger;

        public GetRecommendedTitlesQueryHandler(
            IRecommendedTitlesRepository recomendedTitlesRepository, 
            ICastRepository castRepository,
            ITitleGenreRepository titleGenreRepository,
            IGenreRepository genreRepository)
        {
            this.recomendedTitlesRepository = recomendedTitlesRepository;
            this.castRepository = castRepository;
            this.titleGenreRepository = titleGenreRepository;
            this.genreRepository = genreRepository;
        }

        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetRecommendedTitlesQuery query, CancellationToken cancellationToken)
        {
            IQueryable<RecommendedTitle> titles = recomendedTitlesRepository.Query();

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            IReadOnlyCollection<Genre> allGenres = await genreRepository.GetAll();

            IReadOnlyCollection<TitleResponse> response = new List<TitleResponse>();

            if (query.TitlesToExclude != null)
            {
                titles = titles.AsEnumerable().ExceptBy(query.TitlesToExclude, t => t.Id).AsQueryable();
            }

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

                titles = titles.Where(title =>
                    (includeGenres.Count == 0 || includeGenres.Any(genre => title.Genres.Contains(genre)))
                    && (excludeGenres.Count == 0 || !excludeGenres.Any(genre => title.Genres.Contains(genre)))
                    && (mustIncludeGenres.Count == 0 || mustIncludeGenres.All(genre => title.Genres.Contains(genre)))
                );
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
                                                       .Select(c => c.TitleId)
                                                       .Distinct();

                        matchingTitleIds = matchingTitleIds.Any() ? matchingTitleIds.Intersect(matchedTitles) : matchedTitles;
                    }

                    titles = titles.Where(title => matchingTitleIds.Contains(title.Id));
                }
            }

            if (query.MinimumRating != null)
            {
                titles = titles.Where (t => t.AverageRating >= query.MinimumRating);
            }

            if (query.YearsRange != null)
            {
                query.YearsRange = BubbleSort(query.YearsRange, query.YearsRange.Length);

                titles = titles.Where(t => 
                    t.ReleaseDate.HasValue &&
                    t.ReleaseDate.Value.Year > query.YearsRange[0] &&
                    t.ReleaseDate.Value.Year < query.YearsRange[1]);
            }

            IReadOnlyCollection<RecommendedTitle> data = titles
                .AsEnumerable()
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
