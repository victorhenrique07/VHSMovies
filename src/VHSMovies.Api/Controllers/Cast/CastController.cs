using Microsoft.AspNetCore.Mvc;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Api.Controllers.Cast
{
    [ApiController]
    [Route("cast/")]
    public class CastController : ControllerBase
    {
        private readonly IMediator mediator;
        private IConfiguration configuration;

        private readonly ILogger<CastController> logger;

        public CastController(IMediator mediator, IConfiguration configuration, ILogger<CastController> logger)
        {
            this.mediator = mediator;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet("actors")]
        public async Task<IActionResult> GetActors()
        {
            GetPeopleCommand command = new GetPeopleCommand()
            {
                Role = "Actor"
            };

            IReadOnlyCollection<PersonResponse> actors = await mediator.Send(command);

            return Ok(actors);
        }

        [HttpGet("directors")]
        public async Task<IActionResult> GetDirectors()
        {
            GetPeopleCommand command = new GetPeopleCommand()
            {
                Role = "Director"
            };

            IReadOnlyCollection<PersonResponse> directors = await mediator.Send(command);

            return Ok(directors);
        }

        [HttpGet("writers")]
        public async Task<IActionResult> GetWriters()
        {
            GetPeopleCommand command = new GetPeopleCommand()
            {
                Role = "Writers"
            };

            IReadOnlyCollection<PersonResponse> writers = await mediator.Send(command);

            return Ok(writers);
        }
    }
}
