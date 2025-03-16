using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
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

namespace VHSMovies.Application.Handlers
{
    public class GetMostRelevantTitlesQueryHandler : IRequestHandler<GetMostRelevantTitlesQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly IGenreRepository genreRepository;
        private readonly IRecomendedTitlesRepository recommendedTitlesRepository;
        private readonly IMemoryCache _cache;

        public GetMostRelevantTitlesQueryHandler(IRecomendedTitlesRepository recommendedTitlesRepository, IMemoryCache _cache, IGenreRepository genreRepository)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
            this._cache = _cache;
            this.genreRepository = genreRepository;
        }
        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetMostRelevantTitlesQuery query, CancellationToken cancellationToken)
        {
            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            if (query.GenresId != null && query.GenresId.Any())
            {
                var genresToQuery = await genreRepository.Query()
                    .Where(g => query.GenresId.Contains(g.Id))
                    .Select(g => g.Name.ToLower())
                    .ToListAsync(cancellationToken);

                return await recommendedTitlesRepository.Query()
                    .Where(t => genresToQuery.Any(genre => EF.Functions.Like(t.Genres.ToLower(), "%" + genre + "%")))
                    .OrderByDescending(t => t.Relevance)
                    .Take(query.TitlesAmount)
                    .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                    .ToListAsync(cancellationToken);
            }

            return await recommendedTitlesRepository.Query()
                .OrderByDescending(t => t.Relevance)
                .Take(query.TitlesAmount)
                .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                .ToListAsync(cancellationToken);
        }
    }
}
