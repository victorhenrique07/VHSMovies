﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;

namespace VHSMovies.Application.Commands
{
    public class ReadMoviesCommand : IRequest<Unit>
    {
        public List<Dictionary<string, string>> TitlesRows { get; set; }

        public bool isCsv { get; set; }
    }
}
