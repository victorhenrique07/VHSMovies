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

        public TVShow(string name, DateOnly? releaseDate, string description, string principalImageUrl, string posterImageUrl, List<Review> ratings) :
            base(name, releaseDate, description, principalImageUrl, posterImageUrl, ratings)
        {
        }

        public ICollection<TVShowSeason> Seasons { get; set; } = new List<TVShowSeason>();
    }
}
