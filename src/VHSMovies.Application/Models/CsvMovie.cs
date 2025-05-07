using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Models
{
    public class CsvMovie
    {
        public string tconst { get; set; }
        public string titleType { get; set; }
        public string primaryTitle { get; set; }
        public string originalTitle { get; set; }
        public string isAdult { get; set; }
        public string startYear { get; set; }
        public string endYear { get; set; }
        public string runtimeMinutes { get; set; }
        public string genres { get; set; }
    }
}
