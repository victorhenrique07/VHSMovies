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
            List<Review> ratings, decimal? duration) :
            base(name, description, ratings)
        {
            this.Duration = duration;
        }

        public int TitleId { get; set; }

        public decimal? Duration { get; set; }
    }
}
