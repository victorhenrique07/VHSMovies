using VHSMovies.Mediator;
using Microsoft.AspNetCore.Mvc;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Mediator.Interfaces;
using VHSMovies.Infrastructure.Redis;
using System.Text.Json;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IRedisRepository redis;

        public GenreController(IMediator mediator, IRedisRepository redis)
        {
            this.mediator = mediator;
            this.redis = redis;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllGenres()
        {
            string cacheKey = "genres";

            var cachedData = await redis.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedResponse = JsonSerializer.Deserialize<IReadOnlyCollection<GenreResponse>>(cachedData);

                return Ok(cachedResponse);
            }

            IReadOnlyCollection<GenreResponse> genres = await mediator.Send(new GetGenresQuery());

            var jsonResponse = JsonSerializer.Serialize(genres);
            await redis.SetAsync(cacheKey, jsonResponse);

            return Ok(genres);
        }
    }
}
