﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TitleGenre> Titles { get; set; } = new List<TitleGenre>();
    }
}
