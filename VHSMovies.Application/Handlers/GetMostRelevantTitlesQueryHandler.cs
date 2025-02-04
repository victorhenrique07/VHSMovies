using MediatR;
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
        private readonly IRecomendedTitlesRepository recommendedTitlesRepository;
        private readonly IMemoryCache _cache;

        public GetMostRelevantTitlesQueryHandler(IRecomendedTitlesRepository recommendedTitlesRepository, IMemoryCache _cache)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
            this._cache = _cache;
        }
        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetMostRelevantTitlesQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<RecommendedTitle> titles = await recommendedTitlesRepository.GetAllRecommendedTitles();

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            List<TitleResponse> response = new List<TitleResponse>();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            if (query.GenreName != null)
            {
                var genreNameLower = query.GenreName.ToLower();
                response = titles
                    .Where(t => t.Genres.ToLower().Contains(genreNameLower))
                    .OrderByDescending(t => t.Relevance)
                    .Take(10)
                    .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                    .ToList();

                _cache.Set($"movies-by-{query.GenreName}", response, cacheOptions);

                return response;
            }

            response = titles
                    .OrderByDescending(t => t.Relevance)
                    .Take(10)
                    .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                    .ToList();

            _cache.Set("most-relevant-movies", response, cacheOptions);

            return response;
        }
    }
}
