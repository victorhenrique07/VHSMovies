using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Movie : Title
    {
        public Movie() { }

        public Movie(string externalId, string name, string description,
            ICollection<TitleDirectors> directors, ICollection<TitleWriters> writers,
            ICollection<Cast> actors, ICollection<TitleGenres> genres,
            ICollection<Review> ratings) :
            base(externalId, name, description, directors,
                writers, actors, genres, ratings)
        {
        }
    }
}
