namespace VHSMovies.Application.Models
{
    public class TitleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal AverageRating { get; set; }

        public int TotalRatings { get; set; }

        public IReadOnlyCollection<GenreResponse> Genres { get; set; }

        public TitleResponse(int id, string name, string description, decimal averageRating, int totalRatings)
        {
            Id = id;
            Name = name;
            Description = description;
            AverageRating = averageRating;
            TotalRatings = totalRatings;
        }
    }
}