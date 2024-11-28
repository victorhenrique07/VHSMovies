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

        public IReadOnlyCollection<Director> Directors { get; set; }

        public IReadOnlyCollection<Writer> Writers { get; set; }

        public IReadOnlyCollection<Actor> Actors { get; set; }

        public IReadOnlyCollection<Genre> Genres { get; set; }

        public IReadOnlyCollection<Review> Ratings { get; set; }

        public Title(string externalId, string name, string description, 
            IReadOnlyCollection<Director> directors, IReadOnlyCollection<Writer> writers, 
            IReadOnlyCollection<Actor> actors, IReadOnlyCollection<Genre> genres,
            IReadOnlyCollection<Review> ratings)
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
