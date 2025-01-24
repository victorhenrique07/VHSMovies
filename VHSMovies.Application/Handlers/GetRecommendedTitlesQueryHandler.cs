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

            if (query.IncludeGenres != null)
            {
                titles = titles.Where(title => title.Genres.Any(genre => query.IncludeGenres.Contains(genre.GenreId)));
            }

            if (query.ExcludeGenres != null)
            {
                titles = titles.Where(title => !title.Genres.Any(genre => query.ExcludeGenres.Contains(genre.GenreId)));
            }

            if (query.Actors != null)
            {
                IEnumerable<Cast> actors = await castRepository.GetCastsByPersonRole(PersonRole.Actor);

                titles = from cast in actors
                         join title in titles
                         on cast.TitleId equals title.Id
                         select title;
            }

            if (query.Directors != null)
            {
                IEnumerable<Cast> directors = await castRepository.GetCastsByPersonRole(PersonRole.Director);

                titles = directors
                .GroupJoin(
                    titles,
                    cast => cast.TitleId,
                    title => title.Id,
                    (cast, matchingTitles) => matchingTitles
                )
                .SelectMany(list => list)
                .GroupBy(title => title.Id)
                .Select(group => group.First());
            }

            if (query.Writers != null)
            {
                IEnumerable<Cast> writers = await castRepository.GetCastsByPersonRole(PersonRole.Writer);

                titles = writers
                    .GroupJoin(
                    titles,
                    cast => cast.TitleId,
                    title => title.Id,
                    (cast, matchingTitles) => matchingTitles
                )
                .SelectMany(list => list)
                .GroupBy(title => title.Id)
                .Select(group => group.First());
            }

            response = titles
                .Select(t => 
                    new TitleResponse(t.Id, t.Name, t.Description) 
                    { 
                        Genres = t.Genres.Select(g => 
                            new GenreResponse(g.Genre.Id, g.Genre.Name)).ToList() 
                    }).ToList();

            return response;
        }
    }
}
