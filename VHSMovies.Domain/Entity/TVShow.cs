using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class TVShow : Title
    {
        public TVShow()
        {
        }

        public TVShow(string externalId, string name, string description,
            ICollection<TitleDirectors> directors, ICollection<TitleWriters> writers,
            ICollection<Cast> actors, ICollection<TitleGenres> genres,
            ICollection<TVShowSeason> seasons, ICollection<Review> ratings) :
            base(externalId, name, description, directors,
                writers, actors, genres, ratings)
        {
            Seasons = seasons;
        }

        public ICollection<TVShowSeason> Seasons { get; set; }
    }
}
