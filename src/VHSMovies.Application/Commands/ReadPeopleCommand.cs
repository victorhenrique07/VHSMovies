using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Commands
{
    public class ReadPeopleCommand : IRequest<Unit>
    {
        public List<Dictionary<string, string>> PeopleRows { get; set; }
    }
}
