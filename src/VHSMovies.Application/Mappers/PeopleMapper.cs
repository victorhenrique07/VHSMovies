﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Mappers
{
    public class PeopleMapper : AutoMapper.Profile
    {
        public PeopleMapper()
        {
            CreateMap<Person, IReadOnlyCollection<PersonResponse>>();
        }
    }
}
