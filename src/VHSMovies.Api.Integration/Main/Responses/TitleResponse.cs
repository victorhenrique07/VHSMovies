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

        public string PosterImageUrl { get; set; }
        public string BackdropImageUrl { get; set; }

        public IReadOnlyCollection<GenreResponse> Genres { get; set; }
        public IReadOnlyCollection<WatchProvider> Providers { get; set; }

        public IReadOnlyCollection<PersonResponse> Directors { get; set; }
        public IReadOnlyCollection<PersonResponse> Actors { get; set; }
        public IReadOnlyCollection<PersonResponse> Writers { get; set; }

    }
}
