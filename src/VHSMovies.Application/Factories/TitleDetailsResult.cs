using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Factories
{
    public class TitleDetailsResult
    {
        public int id { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
        public DateOnly? release_date { get; set; }
        public DateOnly? first_air_date { get; set; }
    }
}
