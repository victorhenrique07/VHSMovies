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

        public ICollection<TitleDirectors> Directors { get; set; } = new List<TitleDirectors>();

        public ICollection<TitleWriters> Writers { get; set; }

        public ICollection<Cast> Actors { get; set; } = new List<Cast>();

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        public ICollection<Review> Ratings { get; set; }

        public Title()
        {
        }

        public Title(string externalId, string name, string description, ICollection<Genre> genres,
            ICollection<Review> ratings)
        {
            ExternalId = externalId;
            Name = name;
            Description = description;
            Genres = genres;
            Ratings = ratings;
        }
    }
}
