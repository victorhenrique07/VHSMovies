using MediatR;
using Microsoft.AspNetCore.Mvc;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("titles/")]
    public class TitleController : ControllerBase
    {
        private readonly IMediator mediator;
        private IConfiguration configuration;

        private readonly ILogger<TitleController> logger;

        public TitleController(
            IMediator mediator,
            IConfiguration configuration,
            ILogger<TitleController> logger)
        {
            this.mediator = mediator;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllTitles([FromQuery] List<string> genres)
        {
            GetTitlesCommand command = new GetTitlesCommand()
            {
                Genres = genres
            };

            IReadOnlyCollection<TitleResponse> titles = await mediator.Send(command);

            return Ok(titles);
        }
    }
}
