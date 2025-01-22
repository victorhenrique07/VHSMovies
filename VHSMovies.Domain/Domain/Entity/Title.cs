using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Title
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Review> Ratings { get; set; } = new List<Review>();

        public Title()
        {
        }

        public Title(string name, string description, List<Review> ratings)
        {
            Name = name;
            Description = description;
            Ratings = ratings;
        }

        public override string ToString()
        {
            return $"{this.Id} - {this.Name}";
        }
    }
}
