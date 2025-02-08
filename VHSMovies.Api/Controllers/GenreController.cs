using MediatR;
using Microsoft.AspNetCore.Mvc;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenreController : ControllerBase
    {
        private readonly IMediator mediator;

        public GenreController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllGenres()
        {
            IReadOnlyCollection<GenreResponse> genres = await mediator.Send(new GetGenresQuery()); 

            return Ok(genres);
        }
    }
}
