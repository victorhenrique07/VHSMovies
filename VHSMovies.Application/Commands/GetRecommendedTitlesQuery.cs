using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;

namespace VHSMovies.Application.Commands
{
    public class GetRecommendedTitlesQuery : IRequest<IReadOnlyCollection<TitleResponse>>
    {
        public int[]? Genres { get; set; }

        public IEnumerable<decimal>? Ratings { get; set; }

        public IReadOnlyCollection<string>? Directors { get; set; }

        public IReadOnlyCollection<string>? Actors { get; set; }

        public IReadOnlyCollection<string>? Writers { get; set; }
    }
}
