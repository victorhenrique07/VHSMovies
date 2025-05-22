using System.Linq;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infrastructure.Redis;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Api.Controllers.Recommend
{
    [ApiController]
    [Route("api/titles")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IRedisRepository redis;

        public RecommendationsController(IMediator mediator, IRedisRepository redis)
        {
            this.mediator = mediator;
            this.redis = redis;
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> RecommendedTitles(string? includeGenres, string? mustInclude, string? excludeGenres,
            decimal? minimumRating, string? yearsRange, int? titlesAmount, string? titlesToExclude, string? types)
        {
            var cacheKey = GenerateCacheKey(includeGenres, mustInclude, excludeGenres, minimumRating.ToString(),
                yearsRange, titlesAmount.ToString(), titlesToExclude, types);

            var cachedData = await redis.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedResponse = JsonSerializer.Deserialize<IReadOnlyCollection<TitleResponse>>(cachedData);

                return Ok(cachedResponse);
            }

            GetRecommendedTitlesQuery query = new GetRecommendedTitlesQuery()
            {
                IncludeGenres = includeGenres != null ? ParseStringIntoHashSet(includeGenres) : null,
                ExcludeGenres = excludeGenres != null ? ParseStringIntoHashSet(excludeGenres) : null,
                MustInclude = mustInclude != null ? ParseStringIntoHashSet(mustInclude) : null,
                MinimumRating = minimumRating,
                YearsRange = yearsRange != null ? ParseStringIntoIntArray(yearsRange) : null,
                TitlesAmount = titlesAmount != null ? titlesAmount.Value : 10,
                TitlesToExclude = titlesToExclude != null ? ParseStringIntoIntArray(titlesToExclude) : null,
                Types = types != null ? ParseStringIntoIntArray(types) : null
            };

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            var jsonResponse = JsonSerializer.Serialize(response);
            await redis.SetAsync(cacheKey, jsonResponse);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTitleById(int id)
        {
            GetTitleByIdQuery query = new GetTitleByIdQuery() { Id = id };

            TitleResponse? response = await mediator.Send(query);

            if (response == null)
                return NotFound($"Title not found");

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

        private string GenerateCacheKey(params string?[] inputs)
        {
            var rawKey = string.Join("|", inputs.Select(x => x ?? string.Empty));
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(rawKey);
            var hashBytes = sha256.ComputeHash(bytes);
            return "RecommendedTitles_" + BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }
}
