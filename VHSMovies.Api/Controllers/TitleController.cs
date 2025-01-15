using MediatR;
using Microsoft.AspNetCore.Mvc;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("titles/")]
    public class TitleController : ControllerBase
    {
        private readonly IMediator mediator;
        private IConfiguration configuration;

        public TitleController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        /*[HttpGet("")]
        public async Task<IActionResult> GetTitles()
        {


        }*/
    }
}
