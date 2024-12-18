using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Title
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Cast> Cast { get; set; } = new List<Cast>();

        public ICollection<TitleGenre> Genres { get; set; } = new List<TitleGenre>();

        public List<Review> Ratings { get; set; }

        public string Url { get; set; }

        public Title()
        {
        }

        public Title(string externalId, string name, string description, List<Review> ratings)
        {
            ExternalId = externalId;
            Name = name;
            Description = description;
            Ratings = ratings;
        }
    }
}
