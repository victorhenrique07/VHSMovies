using VHSMovies.Mediator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Mediator.Interfaces;
using VHSMovies.Mediator.Implementation;

namespace VHSMovies.Application.Commands
{
    public class ReadReviewsCommand : IRequest<Unit>
    {
        public List<Dictionary<string, string>> ReviewsRows { get; set; }

        public bool isCsv { get; set; }
    }
}
