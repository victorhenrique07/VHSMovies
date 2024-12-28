using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Api.Integration.Detail
{
    public class TitleDetails
    {
        public string Name { get; set; }

        public IDictionary<string, decimal> Rating { get; set; }

        public string Description { get; set; }

        public List<string> Genres { get; set; }
    }
}
