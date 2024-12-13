using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class TVShowSeason
    {
        public TVShowSeason(int episodesQuantity)
        {
            EpisodesQuantity = episodesQuantity;
        }

        public int Id { get; set; }

        public int EpisodesQuantity { get; set; }
    }
}
