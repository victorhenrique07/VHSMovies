using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
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

        public ICollection<TitleGenres> Genres { get; set; } = new List<TitleGenres>();

        public ICollection<Review> Ratings { get; set; }

        public Title()
        {
        }

        public Title(string externalId, string name, string description,
            ICollection<TitleDirectors> directors, ICollection<TitleWriters> writers,
            ICollection<Cast> actors, ICollection<TitleGenres> genres,
            ICollection<Review> ratings)
        {
            ExternalId = externalId;
            Name = name;
            Description = description;
            Directors = directors;
            Writers = writers;
            Actors = actors;
            Genres = genres;
            Ratings = ratings;
        }
    }
}
