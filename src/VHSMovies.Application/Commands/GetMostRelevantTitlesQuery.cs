using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;

namespace VHSMovies.Application.Commands
{
    public class GetMostRelevantTitlesQuery : IRequest<IReadOnlyCollection<TitleResponse>>
    {
        public int[]? GenresId { get; set; }

        public int TitlesAmount { get; set; }
    }
}
