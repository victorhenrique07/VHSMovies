﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IGenreRepository
    {
        Task<IReadOnlyCollection<Genre>> GetAll();
    }
}
