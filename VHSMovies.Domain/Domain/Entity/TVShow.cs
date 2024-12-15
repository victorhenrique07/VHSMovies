using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class TVShow : Title
    {
        public TVShow()
        {
        }

        public TVShow(string externalId, string name, string description,
            Cast cast, ICollection<Genre> genres, List<Review> ratings) :
            base(externalId, name, description, cast, genres, ratings)
        {
        }

        public ICollection<TVShowSeason> Seasons { get; set; }
    }
}
