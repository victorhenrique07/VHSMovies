using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class TVShow : Title
    {
        public TVShow(string externalId, string name, string description,
            IReadOnlyCollection<Director> directors, IReadOnlyCollection<Writer> writers,
            IReadOnlyCollection<Actor> actors, IReadOnlyCollection<Genre> genres,
            IReadOnlyCollection<TVShowSeason> seasons, IReadOnlyCollection<Review> ratings) :
            base(externalId, name, description, directors, writers, actors, genres, ratings)
        {
            this.Seasons = seasons;
        }

        public IReadOnlyCollection<TVShowSeason> Seasons { get; set; }
    }
}
