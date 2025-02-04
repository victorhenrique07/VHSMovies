using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        public async Task<IActionResult> RecommendedTitles(string? includeGenres, string? mustInclude, string? excludeGenres, string? ratingsRange)
        {
            if (_cache.TryGetValue("movies", out List<TitleResponse> titles))
            {
                return Ok(titles);
            }

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres != null ? ParseStringIntoIntArray(includeGenres) : null,
                ExcludeGenres = excludeGenres != null ? ParseStringIntoIntArray(excludeGenres) : null,
                MustInclude = mustInclude != null ? ParseStringIntoIntArray(mustInclude) : null
                //Ratings = ParseStringIntoDecimalList(ratingsRange)
            };

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("most-relevant")]
        public async Task<IActionResult> GetMostRelevantTitles()
        {
            if (_cache.TryGetValue("most-relevant-movies", out List<TitleResponse> titles))
            {
                return Ok(titles);
            }

            GetMostRelevantTitlesQuery query = new GetMostRelevantTitlesQuery();

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("most-relevant/{genre}")]
        public async Task<IActionResult> GetMostRelevantTitlesByGenre(string genre)
        {
            if (_cache.TryGetValue($"movies-by-{genre}", out List<TitleResponse> titles))
            {
                return Ok(titles);
            }

            GetMostRelevantTitlesQuery query = new GetMostRelevantTitlesQuery() { GenreName = genre };

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

        private HashSet<int> ParseStringIntoIntArray(string data)
        {
            HashSet<int> list = data
                .Split(",")
                .Select(s => int.Parse(s.Trim()))
                .ToHashSet();

            return list;
        }

        private List<decimal> ParseStringIntoDecimalList(string data)
        {
            List<decimal> list = data
                .Split(",")
                .Select(s => decimal.Parse(s.Trim()))
                .ToList();

            return list;
        }
    }
}
