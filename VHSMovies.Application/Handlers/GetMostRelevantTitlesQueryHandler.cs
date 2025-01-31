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
    public class GetMostRelevantTitlesQueryHandler : IRequestHandler<GetMostRelevantTitlesQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly ITitleRepository<Title> titleRepository;

        public GetMostRelevantTitlesQueryHandler(ITitleRepository<Title> titleRepository)
        {
            this.titleRepository = titleRepository;
        }
        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetMostRelevantTitlesQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Title> titles = await titleRepository.GetAll();

            if (query.GenreId != null)
            {
                titles = titles
                .Where(t => t.Genres.Any(g => g.GenreId == query.GenreId));
            }

            return titles
                .OrderByDescending(t => t.Relevance)
                .Take(10)
                .Select(t =>
                    new TitleResponse(t.Id, t.Name, t.Description, t.MedianRate, t.TotalRatings)
                    {
                        PrincipalImageUrl = t.PrincipalImageUrl,
                        PosterImageUrl = t.PosterImageUrl,
                        Genres = t.Genres.Select(g =>
                            new GenreResponse(g.Genre.Id, g.Genre.Name)).ToList()
                    })
                .ToList();
        }
    }
}
