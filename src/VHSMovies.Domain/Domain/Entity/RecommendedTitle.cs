using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class RecommendedTitle
    {
        public RecommendedTitle()
        {
        }

        public int Id { get; set; }

        public int Type { get; set; }

        public string IMDB_Id { get; set; }

        public string Name { get; set; }

        public int? ReleaseDate { get; set; }

        public int TotalReviews { get; set; }

        public decimal AverageRating { get; set; }

        public decimal Relevance { get; set; }

        public string[] Genres { get; set; }
    }
}
