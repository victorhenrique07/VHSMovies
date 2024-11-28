using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Movie : Title
    {
        public Movie(string externalId, string name, string description, 
            IReadOnlyCollection<Director> directors, IReadOnlyCollection<Writer> writers, 
            IReadOnlyCollection<Actor> actors, IReadOnlyCollection<Genre> genres,
            IReadOnlyCollection<Review> ratings) :
            base(externalId, name, description, directors, writers, actors, genres, ratings)
        {
        }
    }
}
