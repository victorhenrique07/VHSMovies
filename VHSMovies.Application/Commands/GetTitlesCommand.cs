using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;

namespace VHSMovies.Application.Commands
{
    public class GetTitlesCommand : IRequest<IReadOnlyCollection<TitleResponse>>
    {
        public List<string> Genres { get; set; }
    }
}
