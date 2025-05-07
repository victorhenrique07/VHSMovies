using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Api.Integration.Main.Queries
{
    public class GetRecommendedTitlesQuery
    {
        public int[]? MustInclude { get; set; }
        public int[]? IncludeGenres { get; set; }
        public int[]? ExcludeGenres { get; set; }

        public decimal MinimumRating { get; set; }

        public int[]? YearsRange { get; set; }

        public int? TitlesAmount { get; set; }

        public IReadOnlyCollection<string>? Directors { get; set; }

        public IReadOnlyCollection<string>? Actors { get; set; }

        public IReadOnlyCollection<string>? Writers { get; set; }

        public IReadOnlyCollection<int>? TitlesToExclude { get; set; }

        public int[]? Types { get; set; }
    }
}
