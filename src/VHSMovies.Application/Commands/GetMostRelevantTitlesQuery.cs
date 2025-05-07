using VHSMovies.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;
using VHSMovies.Mediator.Interfaces;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Commands
{
    public class GetMostRelevantTitlesQuery : IRequest<IReadOnlyCollection<TitleResponse>>
    {
        public int[]? GenresId { get; set; }

        public int TitlesAmount { get; set; }

        public int[]? Types { get; set; }

        public IReadOnlyCollection<int>? TitlesToExclude { get; set; }
    }
}
