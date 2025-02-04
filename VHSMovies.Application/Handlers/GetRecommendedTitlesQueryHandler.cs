using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class GetRecommendedTitlesQueryHandler : IRequestHandler<GetRecommendedTitlesQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly ITitleRepository<Title> titleRepository;
        private readonly ICastRepository castRepository;
        private readonly ITitleGenreRepository titleGenreRepository;
        private readonly ILogger<GetRecommendedTitlesQueryHandler> logger;

        public GetRecommendedTitlesQueryHandler(
            ITitleRepository<Title> titleRepository, 
            ICastRepository castRepository,
            ITitleGenreRepository titleGenreRepository)
        {
            this.titleRepository = titleRepository;
            this.castRepository = castRepository;
            this.titleGenreRepository = titleGenreRepository;
        }

        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetRecommendedTitlesQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Title> titles = await titleRepository.GetAll();

            IReadOnlyCollection<TitleResponse> response = new List<TitleResponse>();

            if ((query.IncludeGenres?.Any() == true) || (query.ExcludeGenres?.Any() == true) || (query.MustInclude?.Any() == true))
            {
                var includeGenres = query.IncludeGenres ?? new HashSet<int>();
                var excludeGenres = query.ExcludeGenres ?? new HashSet<int>();
                var mustInclude = query.MustInclude ?? new HashSet<int>();

                titles = titles.Where(title =>
                    (includeGenres.Count == 0 || title.Genres.Any(genre => includeGenres.Contains(genre.Genre.Id)))
                    && (excludeGenres.Count == 0 || !title.Genres.Any(genre => excludeGenres.Contains(genre.GenreId)))
                    && (mustInclude.Count == 0 || mustInclude.All(id => title.Genres.Select(g => g.Genre.Id).Contains(id)))
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

            titles = titles
                .OrderByDescending(t => t.Relevance)
                .Take(10)
                .ToList();

            response = titles
                .Select(t => 
                    new TitleResponse(t.Id, t.Name, t.Description, t.AverageRating, t.TotalReviews) 
                    {
                        PosterImageUrl = t.PosterImageUrl,
                        PrincipalImageUrl = t.PrincipalImageUrl,
                        Genres = t.Genres.Select(g => 
                            new GenreResponse(g.Genre.Name)).ToList()
                    }).ToList();

            return response;
        }
    }
}
