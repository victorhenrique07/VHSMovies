using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Api.Integration.Main.Queries
{
    public class GetMostRelevantTitlesQuery
    {
        public int[]? GenresId { get; set; }

        public int? TitlesAmount { get; set; }

        public IReadOnlyCollection<int>? TitlesToExclude { get; set; }
    }
}
