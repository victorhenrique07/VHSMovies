using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;

namespace VHSMovies.Api.Controllers.Recommend
{
    [ApiController]
    [Route("api/titles")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IMediator mediator;
        private IConfiguration configuration;
        private readonly IMemoryCache _cache;
        public RecommendationsController(IMediator mediator, IConfiguration configuration, IMemoryCache _cache)
        {
            this.mediator = mediator;
            this.configuration = configuration;
            this._cache = _cache;
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> RecommendedTitles(string? includeGenres, string? mustInclude, string? excludeGenres, decimal? minimumRating, string? yearsRange, int? titlesAmount)
        {
            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres != null ? ParseStringIntoHashSet(includeGenres) : null,
                ExcludeGenres = excludeGenres != null ? ParseStringIntoHashSet(excludeGenres) : null,
                MustInclude = mustInclude != null ? ParseStringIntoHashSet(mustInclude) : null,
                MinimumRating = minimumRating,
                YearsRange = yearsRange != null ? ParseStringIntoIntArray(yearsRange) : null,
                TitlesAmount = titlesAmount != null ? titlesAmount.Value : 10
            };

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("most-relevant")]
        public async Task<IActionResult> GetMostRelevantTitles([FromQuery] string? genresId, int titlesAmount = 10)
        {
            string genres = genresId != null ? genresId.Replace(",", "-").Trim() : "none";

            string cacheKey = $"most-relevant-movies-{genres}-{titlesAmount}";

            if (_cache.TryGetValue(cacheKey, out List<TitleResponse> cachedTitles))
            {
                return Ok(cachedTitles);
            }

            GetMostRelevantTitlesQuery query = new GetMostRelevantTitlesQuery()
            {
                GenresId = genresId != null ? ParseStringIntoIntArray(genresId) : null,
                TitlesAmount = titlesAmount
            };

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("most-relevant/{genreId}")]
        public async Task<IActionResult> GetMostRelevantTitlesByGenre(int genreId)
        {
            if (_cache.TryGetValue($"movies-by-{genreId}", out List<TitleResponse> titles))
            {
                return Ok(titles);
            }

            GetMostRelevantTitlesQuery query = new GetMostRelevantTitlesQuery() { GenresId = [genreId], TitlesAmount = 10 };

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTitleById(int id)
        {
            if (_cache.TryGetValue("single-movie", out TitleResponse title))
            {
                return Ok(title);
            }

            GetTitleByIdQuery query = new GetTitleByIdQuery() { Id = id };

            TitleResponse response = await mediator.Send(query);

            return Ok(response);
        }

        private HashSet<int> ParseStringIntoHashSet(string data)
        {
            HashSet<int> list = data
                .Split(",")
                .Select(s => int.Parse(s.Trim()))
                .ToHashSet();

            return list;
        }

        private int[] ParseStringIntoIntArray(string data)
        {
            int[] list = data
                .Split(",")
                .Select(s => int.Parse(s.Trim()))
                .ToArray();

            return list;
        }

        private List<decimal> ParseStringIntoDecimalList(string data)
        {
            List<decimal> list = data
                .Split(",")
                .Select(s => decimal.Parse(s.Trim()))
                .OrderDescending()
                .ToList();

            return list;
        }
    }
}
