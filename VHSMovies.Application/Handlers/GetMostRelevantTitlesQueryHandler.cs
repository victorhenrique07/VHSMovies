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
            IReadOnlyCollection<Genre> allGenres = await genreRepository.GetAll();

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            List<TitleResponse> response = new List<TitleResponse>();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            if (query.GenresId != null)
            {
                List<string> genresToQuery = allGenres
                    .Where(x => query.GenresId.Contains(x.Id))
                    .Select(x => x.Name.ToLower())
                    .ToList();

                response = (await recommendedTitlesRepository.GetAllRecommendedTitles())
                    .Where(t => t.Genres.ToLower().Split(',')
                        .Select(genre => genre.Trim())
                        .Any(genre => genresToQuery.Contains(genre)))
                    .OrderByDescending(t => t.Relevance)
                    .Take(query.TitlesAmount)
                    .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                    .ToList();

                //_cache.Set($"movies-by-{query.GenreName}", response, cacheOptions);

                return response;
            }

            response = (await recommendedTitlesRepository.GetAllRecommendedTitles())
                    .OrderByDescending(t => t.Relevance)
                    .Take(query.TitlesAmount)
                    .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                    .ToList();

            _cache.Set($"most-relevant-movies-{query.TitlesAmount}", response, cacheOptions);

            return response;
        }
    }
}
