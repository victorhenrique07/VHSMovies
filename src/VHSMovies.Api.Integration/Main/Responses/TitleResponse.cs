using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Api.Integration.Main.Responses
{
    public class TitleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public string Description { get; set; }

        public decimal AverageRating { get; set; }

        public int TotalReviews { get; set; }

        public string PrincipalImageUrl { get; set; }

        public string PosterImageUrl { get; set; }

        public IReadOnlyCollection<GenreResponse> Genres { get; set; }
    }
}
