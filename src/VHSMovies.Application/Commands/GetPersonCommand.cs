﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;

namespace VHSMovies.Application.Commands
{
    public class GetPersonCommand : IRequest<PersonResponse>
    {
        public int PersonId { get; set; }
    }
}
