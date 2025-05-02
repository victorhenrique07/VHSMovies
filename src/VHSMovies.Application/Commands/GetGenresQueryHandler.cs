using VHSMovies.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Commands
{
    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IReadOnlyCollection<GenreResponse>>
    {
        private readonly IGenreRepository genreRepository;

        public GetGenresQueryHandler(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<IReadOnlyCollection<GenreResponse>> Handle(GetGenresQuery query, CancellationToken cancellationToken)
        {
            var genres = await genreRepository.GetAll();

            return genres.Select(t =>
                    new GenreResponse(t.Name) { Id = t.Id }
                ).ToList();
        }
    }
}
