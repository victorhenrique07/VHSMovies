namespace VHSMovies.Application.Models
{
    public class TitleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public decimal AverageRating { get; set; }

        public int TotalReviews { get; set; }

        public string PosterImageUrl { get; set; }

        public string BackdropImageUrl { get; set; }

        public IReadOnlyCollection<GenreResponse> Genres { get; set; }

        public ICollection<CastResponse> Actors { get; set; } = new List<CastResponse>();
        public ICollection<CastResponse> Directors { get; set; } = new List<CastResponse>();
        public ICollection<CastResponse> Writers { get; set; } = new List<CastResponse>();

        public List<WatchProvider> Providers { get; set; }

        public TitleResponse()
        {
        }

        public TitleResponse(int id, string name, DateOnly? releaseDate, string description, decimal averageRating, int totalRatings)
        {
            Id = id;
            Name = name;
            ReleaseDate = releaseDate;
            Description = description;
            AverageRating = averageRating;
            TotalReviews = totalRatings;
        }
    }
}