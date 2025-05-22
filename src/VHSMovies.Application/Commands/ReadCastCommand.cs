using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Mediator;
using VHSMovies.Mediator.Implementation;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Commands
{
    public class ReadCastCommand : IRequest<Unit>
    {
        public List<Dictionary<string, string>> CastRows { get; set; }
    }
}
