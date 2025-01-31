using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public RecommendationsController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> RecommendedTitles(string? includeGenres, string? mustInclude, string? excludeGenres, string? ratingsRange)
        {
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
            GetMostRelevantTitlesQuery query = new GetMostRelevantTitlesQuery();

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("most-relevant/{genreId}")]
        public async Task<IActionResult> GetMostRelevantTitlesByGenre(int genreId)
        {
            GetMostRelevantTitlesQuery query = new GetMostRelevantTitlesQuery() { GenreId = genreId };

            IReadOnlyCollection<TitleResponse> response = await mediator.Send(query);

            return Ok(response);
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
                .ToList();

            return list;
        }
    }
}
