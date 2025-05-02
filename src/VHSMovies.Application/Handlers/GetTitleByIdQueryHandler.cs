using VHSMovies.Mediator;
using Microsoft.Extensions.Caching.Memory;
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
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, TitleResponse>
    {
        private readonly IRecommendedTitlesRepository recommendedTitlesRepository;
        private readonly IMemoryCache _cache;

        public GetTitleByIdQueryHandler(IRecommendedTitlesRepository recommendedTitlesRepository, IMemoryCache _cache)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
            this._cache = _cache;
        }
        public async Task<TitleResponse> Handle(GetTitleByIdQuery query, CancellationToken cancellationToken)
        {
            RecommendedTitle title = await recommendedTitlesRepository.GetById(query.Id);

            title.AverageRating = title.AverageRating / 2;

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            TitleResponse response = titleResponseFactory.CreateTitleResponseByRecommendedTitle(title);

            _cache.Set($"single-movie", response, cacheOptions);

            return response;
        }
    }
}
