namespace VHSMovies.Application.Models
{
    public class TitleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public decimal AverageRating { get; set; }

        public int TotalRatings { get; set; }

        public string PosterImageUrl { get; set; }
        public string BackdropImageUrl { get; set; }

        public IReadOnlyCollection<GenreResponse> Genres { get; set; }

        public IReadOnlyCollection<CastResponse> Cast { get; set; }

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
            TotalRatings = totalRatings;
        }
    }
}