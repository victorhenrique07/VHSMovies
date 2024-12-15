using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Movie : Title
    {
        public Movie() { }

        public Movie(string externalId, string name, string description,
            Cast cast, ICollection<Genre> genres,
            List<Review> ratings) :
            base(externalId, name, description, cast, genres, ratings)
        {
        }
    }
}
