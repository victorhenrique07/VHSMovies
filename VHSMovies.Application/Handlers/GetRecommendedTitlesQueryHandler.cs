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
            IEnumerable<Title> titles = (await titleRepository.GetAll()).ToList();

            IReadOnlyCollection<TitleResponse> response = new List<TitleResponse>();

            if (query.Genres != null)
            {
                IEnumerable<TitleGenre> genres = (await titleGenreRepository.GetTitlesByGenreId(query.Genres)).ToList(); // Materializa os 15 objetos.

                titles = genres
                    .GroupJoin(
                        titles,
                        genre => genre.TitleId,
                        title => title.Id,
                        (genre, matchingTitles) => matchingTitles
                    )
                    .SelectMany(lsit => lsit)
                    .GroupBy(title => title.Id)
                    .Select(group => group.First());
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
                IEnumerable<Cast> actors = await castRepository.GetCastsByPersonRole(PersonRole.Director);

                titles = from cast in actors
                         join title in titles
                         on cast.TitleId equals title.Id
                         select title;
            }

            if (query.Writers != null)
            {
                IEnumerable<Cast> actors = await castRepository.GetCastsByPersonRole(PersonRole.Writer);

                titles = from cast in actors
                         join title in titles
                         on cast.TitleId equals title.Id
                         select title;
            }

            response = titles.ToList().Select(t => new TitleResponse(t.Id, t.Name, t.Description)).ToList();

            return response;
        }
    }
}
