using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Title
    {
        public int TitleId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Cast> Cast { get; set; } = new List<Cast>();

        public ICollection<TitleGenre> Genres { get; set; } = new List<TitleGenre>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public Title()
        {
        }

        public Title(string name, string description, List<Review> reviews)
        {
            Name = name;
            Description = description;
            Reviews = reviews;
        }
    }
}
