﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Mediator;
using VHSMovies.Mediator.Implementation;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Commands
{
    public class ReadReviewsCommand : IRequest<Unit>
    {
        public List<Dictionary<string, string>> ReviewsRows { get; set; }

        public bool isCsv { get; set; }

        public string Source { get; set; }
    }
}
