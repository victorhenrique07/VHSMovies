namespace VHSMovies.Application.Models
{
    public class TitleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Genres { get; set; }

        public TitleResponse(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}