using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Commands
{
    public class GetRecommendedTitlesQuery : IRequest<IReadOnlyCollection<TitleResponse>>
    {
        public HashSet<int>? MustInclude { get; set; }
        public HashSet<int>? IncludeGenres { get; set; }
        public HashSet<int>? ExcludeGenres { get; set; }

        public decimal? MinimumRating { get; set; }

        public int[]? YearsRange { get; set; }

        public IReadOnlyCollection<string>? Directors { get; set; }

        public IReadOnlyCollection<string>? Actors { get; set; }

        public IReadOnlyCollection<string>? Writers { get; set; }

        public IReadOnlyCollection<int>? TitlesToExclude { get; set; }

        public int[]? Types { get; set; }

        public int TitlesAmount { get; set; } = 10;
    }
}
