using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using VHSMovies.Application.Models;
using VHSMovies.Application.Commands;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("/api/search")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator mediator;

        public SearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string query)
        {
            IReadOnlyCollection<TitleResponse> results = await mediator.Send(new GetTitlesBySearchQuery() { SearchQuery = query });

            return Ok(results);
        }
    }
}