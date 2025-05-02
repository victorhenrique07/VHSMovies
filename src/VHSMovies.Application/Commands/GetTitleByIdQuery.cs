using VHSMovies.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Commands
{
    public class GetTitleByIdQuery : IRequest<TitleResponse>
    {
        public int Id { get; set; }
    }
}
