using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Models
{
    public class CsvMovie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string runtime { get; set; }
        public string release_date { get; set; }
        public string backdrop_path { get; set; }
        public string tconst { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string genres { get; set; }
        public string averageRating { get; set; }
        public string numVotes { get; set; }
    }
}
