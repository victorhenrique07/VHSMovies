using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class GetTitlesCommandHandler : IRequestHandler<GetTitlesCommand, IReadOnlyCollection<TitleResponse>>
    {
        private readonly ITitleRepository<Title> titleRepository;

        public GetTitlesCommandHandler(ITitleRepository<Title> titleRepository)
        {
            this.titleRepository = titleRepository;
        }

        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetTitlesCommand command, CancellationToken cancellationToken)
        {
            IEnumerable<Title> titles = await titleRepository
                .GetAll();

            if (command.Genres != null)
            {
                titles = titles
                    .Where(t => t.Genres
                        .Select(g => g.Genre.Name)
                        .Any(genre => command.Genres.Contains(genre))
                    );
            }

            return titles
                .Select(t => new TitleResponse(t.Id, t.Name, t.Description)
                {
                    Genres = t.Genres.Select(g => 
                    new GenreResponse() { Name = g.Genre.Name }).ToArray()
                }).ToList();
        }
    }
}
